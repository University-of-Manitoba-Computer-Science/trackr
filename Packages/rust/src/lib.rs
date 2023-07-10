use reqwest::Client;
use std::thread;
use std::time::Duration;

const API_ENDPOINT: &str = "http://wryneck.cs.umanitoba.ca/api/values";

pub async fn add_single_value(api_key: &str, field_id: i32, value: &str) -> Result<String, reqwest::Error> {
    let client = Client::new();
    let data = [("apiKey", api_key), ("value", value), ("fieldId", &field_id.to_string())];
    let response = client.post(API_ENDPOINT).form(&data).send().await?;
    let body = response.text().await?;
    Ok(body)
}

pub async fn add_many_values(api_key: &str, field_id: i32, values: &[&str]) -> Result<String, reqwest::Error> {
    let mut result = String::new();

    for value in values {
        let response = add_single_value(api_key, field_id, value).await?;
        if response != "200" {
            return Ok(response);
        }
        thread::sleep(Duration::from_secs(1));
        result = response;
    }
    Ok(result)
}

pub async fn get_values(api_key: &str, field_id: i32, offset: i32, limit: i32, order: &str) -> Result<String, reqwest::Error> {
    let client = Client::new();
    let params = [
        ("apiKey", api_key),
        ("fieldId", &field_id.to_string()),
        ("offset", &offset.to_string()),
        ("limit", &limit.to_string()),
        ("order", order),
    ];
    let response = client.get(API_ENDPOINT).query(&params).send().await?;
    let body = response.text().await?;
    Ok(body)
}