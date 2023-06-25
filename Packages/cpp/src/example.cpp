#include "../lib/trackr.hpp"
#include <iostream>
#include <unistd.h>

int main(int argc, char* argv[]) {
    if (argc < 2) {
        std::cout << "Usage: " << argv[0] << " <api key>" << std::endl;
        return 1;
    }

    std::string apiKey = std::string(argv[1]);

    // Add single value
    long rc = Trackr::addSingleValue(3, 100, apiKey);

    std::cout << "HTTP response code: " << rc << std::endl;
    
    // You can also add a batch of values (warning: slow! 1s per value due to API rate limiting)
    std::list<int> values = { 100, 200, 300, 400, 500 };
    rc = Trackr::addMultipleValues(3, values, apiKey);

    std::cout << "HTTP response code: " << rc << std::endl;

    // If you dont want to specify the API key in every API call, you can set it globally:
    Trackr::setApiKey(apiKey);

    // Now you can omit the API key in the API calls:
    rc = Trackr::addSingleValue(3, 100);

    std::cout << "HTTP response code: " << rc << std::endl;

    // We can also fetch values from the API:
    Trackr::TrackrValuesResponse* response = Trackr::getValues(3, 0, 10, true);

    std::cout << "Total values: " << response->totalValues << std::endl;

    for (Trackr::TrackrValue* value : *response->values) {
        std::cout << "{ id: " << value->id << ", value: " << value->value << ", createdAt: " << value->createdAt << " }" << std::endl;
    }

    return 0;
}