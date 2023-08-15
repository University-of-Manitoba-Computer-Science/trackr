use reqwest::Client;
use serde::{Deserialize, Serialize};
use std::clone::Clone;
use std::time::{Duration, SystemTime};

const API_ENDPOINT: &str = "http://wryneck.cs.umanitoba.ca/api/values";

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct ApiResponse {
    pub status_code: u16,
    pub text: String,
}

pub struct Trackr {
    api_key: String,
    api_url: String,
    last_call_timestamp: Option<SystemTime>,
}

impl Trackr {
    pub fn new(api_key: String) -> Self {
        Trackr {
            api_key,
            api_url: "http://wryneck.cs.umanitoba.ca/api/values/".to_string(),
            last_call_timestamp: None,
        }
    }

    pub fn set_api_key(&mut self, api_key: String) {
        self.api_key = api_key;
    }

    pub fn set_api_url(&mut self, api_url: String) {
        self.api_url = api_url;
    }

    pub async fn add_single_value(&mut self, field_id: u32, value: u32) -> Result<ApiResponse, reqwest::Error> {
        let client = Client::new();
        let data = [("fieldId", field_id.to_string()), ("value", value.to_string()), ("apiKey", self.api_key.clone())];

        let response = client
        .post(API_ENDPOINT)
        .header("Content-Type", "application/x-www-form-urlencoded")
        .form(&data)
        .send()
        .await?;

        self.last_call_timestamp = Some(SystemTime::now());

        let api_response = ApiResponse {
            status_code: response.status().as_u16(),
            text: response.text().await?,
        };

        Ok(api_response)
    }

    pub async fn add_many_values(&mut self, field_id: u32, values: &[u32]) -> Result<ApiResponse, reqwest::Error> {
        
        let mut api_responses = Vec::new();
        for value in values {
            let api_response = self.add_single_value(field_id, *value).await?;
            api_responses.push(api_response);
            
            if let Some(last_call_timestamp) = self.last_call_timestamp {
                match last_call_timestamp.elapsed() {
                    Ok(elapsed) => {
                        if elapsed < Duration::from_secs(1) {
                            tokio::time::sleep(Duration::from_secs(1) - elapsed).await;
                        }
                    }
                    Err(e) => {
                        // Handle the error case, e.g., print an error message or return an error
                        eprintln!("Error calculating elapsed time: {:?}", e);
                    }
                }
            }
        }

        Ok(api_responses[api_responses.len() - 1].clone())
    }

    pub async fn get_values(
        &self,
        field_id: u32,
        offset: u32,
        limit: u32,
        order: &String,
    ) -> Result<GetValuesResponse, reqwest::Error> {
        let client = Client::new();
        let url = format!(
            "{}?apiKey={}&fieldId={}&offset={}&limit={}&order={}",
            API_ENDPOINT,
            self.api_key,
            field_id,
            offset,
            limit,
            order
        );

        let response = client
        .get(&url)
        .send()
        .await?;

        let api_response = response.json::<GetValuesResponse>().await?;

        Ok(api_response)
    }
}

#[derive(Debug, Serialize, Deserialize)]
pub struct Value {
    pub id: u32,
    pub value: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct GetValuesResponse {
    pub values: Vec<Value>,
    pub totalValues: u32,
}
