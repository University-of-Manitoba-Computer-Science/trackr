use reqwest::Client;
use serde::{Deserialize, Serialize};

const API_ENDPOINT: &str = "http://wryneck.cs.umanitoba.ca/api/values";

#[derive(Debug, Serialize, Deserialize)]
struct ApiResponse {
    status_code: u16,
    text: String,
    // Add other fields as needed
}

pub struct Trackr {
    api_key: String,
    api_url: String,
    last_call_timestamp: time::Duration,
}

impl Trackr {
    pub fn new(api_key: String) -> Self {
        Trackr {
            api_key,
            api_url: "http://wryneck.cs.umanitoba.ca".to_string(),
            last_call_timestamp: time::Duration::from_secs(0),
        }
    }

    pub fn set_api_key(&mut self, api_key: String) {
        self.api_key = api_key;
    }

    pub fn set_api_url(&mut self, api_url: String) {
        self.api_url = api_url;
    }

    pub async fn add_single_value(&self, field_id: u32, value: i32) -> Result<ApiResponse, reqwest::Error> {
        let client = Client::new();
        let data = [
            ("fieldId", field_id.to_string()),
            ("value", value.to_string()),
            ("apiKey", self.api_key.clone()),
        ];

        let response = client.post(API_ENDPOINT).form(&data).send().await?;
        let api_response: ApiResponse = response.json().await?;

        Ok(api_response)
    }

    pub async fn add_many_values(&self, field_id: u32, values: &[i32]) -> Result<ApiResponse, reqwest::Error> {
        let client = Client::new();

        let mut api_responses = Vec::new();
        for value in values {
            let api_response = self.add_single_value(field_id, *value).await?;
            api_responses.push(api_response);
        }

        Ok(api_responses[api_responses.len() - 1].clone())
    }

    pub async fn get_values(
        &self,
        field_id: u32,
        offset: u32,
        limit: u32,
        order: &str,
    ) -> Result<GetValuesResponse, reqwest::Error> {
        let client = Client::new();
        let params = [
            ("apiKey", &self.api_key),
            ("fieldId", &field_id.to_string()),
            ("offset", &offset.to_string()),
            ("limit", &limit.to_string()),
            ("order", order),
        ];

        let response = client.get(API_ENDPOINT).query(&params).send().await?;
        let api_response: GetValuesResponse = response.json().await?;

        Ok(api_response)
    }
}

#[derive(Debug, Serialize, Deserialize)]
struct Value {
    id: u32,
    value: String,
    created_at: String,
    // Add other fields as needed
}

#[derive(Debug, Serialize, Deserialize)]
struct GetValuesResponse {
    values: Vec<Value>,
    total_values: u32,
    // Add other fields as needed
}