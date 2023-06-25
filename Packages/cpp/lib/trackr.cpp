#include "trackr.hpp"
#include <unistd.h>
#include <curl/curl.h>
#include <iostream>
#include <list>
#include "nlohmann/json.hpp"

size_t write_data(char *contents, size_t size, size_t nmemb, void *userp);

std::string Trackr::apiKey = "";
std::string Trackr::apiUrl = "http://wryneck.cs.umanitoba.ca";
time_t Trackr::lastCallTimestamp = 0;

//--------------------------------------------------
// TrackrValue class
//--------------------------------------------------

Trackr::TrackrValue::TrackrValue(int id, int value, std::string timestring) {
    this->id = id;
    this->value = value;
    
    struct tm tm;
    strptime(timestring.c_str(), "%Y-%m-%dT%H:%M:%S", &tm);
    this->createdAt = mktime(&tm);
}

//--------------------------------------------------
// TrackrValuesResponse class
//--------------------------------------------------

Trackr::TrackrValuesResponse::TrackrValuesResponse(int totalValues, std::list<TrackrValue*>* values) {
    this->totalValues = totalValues;
    this->values = values;
}

//--------------------------------------------------
// Namespace functions
//--------------------------------------------------

void Trackr::setApiKey(std::string key) {
    Trackr::apiKey = key;
}

void Trackr::setApiUrl(std::string url) {
    Trackr::apiUrl = url;
}

Trackr::TrackrValuesResponse* Trackr::getValues(int fieldId, int offset, int limit, bool descending, std::string key) {
    CURL *curl = curl_easy_init();
    std::string readBuffer;

    // wait until current time is at least 1s after last call
    time_t now = time(NULL);
    if (now < Trackr::lastCallTimestamp + 1) {
        sleep(Trackr::lastCallTimestamp + 1 - now);
    }
    
    std::string opts = "?fieldId=" + std::to_string(fieldId) + "&offset=" + std::to_string(offset) + "&limit=" + std::to_string(limit) + "&order=" + (descending ? "desc" : "asc") + "&apiKey=" + key;

    curl_easy_setopt(curl, CURLOPT_URL, (Trackr::apiUrl + "/api/values/" + opts).c_str());
    curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, write_data);
    curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

    CURLcode res = curl_easy_perform(curl);

    long http_code = 0;
    curl_easy_getinfo(curl, CURLINFO_RESPONSE_CODE, &http_code);
    
    if (http_code == 200) {
        nlohmann::json j = nlohmann::json::parse(readBuffer);
        std::list<TrackrValue*>* values = new std::list<TrackrValue*>();
        for (nlohmann::json::iterator it = j["values"].begin(); it != j["values"].end(); ++it) {
            values->push_back(new TrackrValue((*it)["id"], stoi((*it)["value"].get_ref<std::string&>()), (*it)["createdAt"]));
        }
        TrackrValuesResponse* response = new TrackrValuesResponse(j["totalValues"], values);
        return response;
    }    

    curl_easy_cleanup(curl);

    Trackr::lastCallTimestamp = time(NULL);

    return nullptr;
}

long Trackr::addSingleValue(int fieldId, int value, std::string key) {
    CURL *curl = curl_easy_init();
    std::string readBuffer;

    // wait until current time is at least 1s after last call
    time_t now = time(NULL);
    if (now < Trackr::lastCallTimestamp + 1) {
        sleep(Trackr::lastCallTimestamp + 1 - now);
    }
    
    std::string opts = "fieldId=" + std::to_string(fieldId) + "&value=" + std::to_string(value) + "&apiKey=" + key;

    curl_easy_setopt(curl, CURLOPT_URL, (Trackr::apiUrl + "/api/values/").c_str());
    curl_easy_setopt(curl, CURLOPT_POSTFIELDS, opts.c_str());
    curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, write_data);
    curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

    CURLcode res = curl_easy_perform(curl);

    long http_code = 0;
    curl_easy_getinfo(curl, CURLINFO_RESPONSE_CODE, &http_code);

    curl_easy_cleanup(curl);

    Trackr::lastCallTimestamp = time(NULL);

    return http_code;
}


long Trackr::addMultipleValues(int fieldId, std::list<int> values, std::string key) {
    long rc;

    for (int i : values) {
        rc = Trackr::addSingleValue(fieldId, i, key);
        if (rc < 200 || rc > 299) {
            return rc;
        }
    }

    return rc;
}

size_t write_data(char *contents, size_t size, size_t nmemb, void *userp)
{
    ((std::string*)userp)->append((char*)contents, size * nmemb);
    return size * nmemb;
}