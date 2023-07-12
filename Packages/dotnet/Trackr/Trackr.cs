using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Trackr
{
    public class Trackr
    {
        private static readonly HttpClient client = new HttpClient();
        private static string ApiEndpoint = "http://wryneck.cs.umanitoba.ca/api/values";

        /// <summary>
        /// Updates the api endpoint to be used. This function is used when hosting a local version of Trackr and therefore needs a different endpoint than the one hosted at the University of Manitoba.
        /// </summary>
        /// <param name="url">The url where the locally hosted version of Trackr is found.</param>
        /// <returns>None</returns>
        public static void UpdateEndpoint(string url)
        {
            ApiEndpoint = url;
        }

        /// <summary>
        /// Add's a single value to the field of the project which has the apikey.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <returns>Returns an HttpResponseMessage with a statusCode and Content detailing the result of the operation.</returns>
        public static async Task<HttpResponseMessage> AddSingleValue(string apiKey, uint fieldId, string value)
        {
            var valueObject = new Dictionary<string, string>
            {
                { "apiKey", apiKey },
                { "value", value },
                { "fieldId", fieldId.ToString() }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(valueObject);
            HttpResponseMessage response = await client.PostAsync(ApiEndpoint, content);
            return response;
        }

        /// <summary>
        /// Accepts a list of string values to be added to the field of the project which has the apiKey.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="fieldId"></param>
        /// <param name="values"></param>
        /// <returns>Returns an HttpResponseMessage with a statusCode and Content detailing the result of the operation.</returns>
        public static async Task<HttpResponseMessage> AddManyValues(string apiKey, uint fieldId, List<string> values)
        {
            foreach (var value in values)
            {
                HttpResponseMessage result = await AddSingleValue(apiKey, fieldId, value);
                if (!result.IsSuccessStatusCode)
                {
                    return result;
                }
                Thread.Sleep(1000);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Accepts configuration parameters which target a project which has the apiKey, to read in "limit" number of values from the field with "fieldId", starting at the "offset" value. Acceptable orders are "asc" or "desc".
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="fieldId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="order"></param>
        /// <returns>Returns an HttpResponseMessage with a statusCode and Content detailing the result of the operation. If the operation was successful, the content will contain "totalValues" which is a number and "values" which is a list of the values returned. Result can be read with 'response.Content.ReadAsStringAsync()' and then parsed.</returns>
        public static async Task<HttpResponseMessage> GetValues(string apiKey, uint fieldId, uint offset, int limit, string order)
        {
            if (!order.ToLower().Equals("asc") && !order.ToLower().Equals("desc"))
            {
                HttpResponseMessage newResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                newResponse.Content = new StringContent("Invalid request parameters provided.");
                return newResponse;
            }

            UriBuilder uriBuilder = new UriBuilder(ApiEndpoint);
            string parameters = "apiKey=" + apiKey + "&fieldId=" + fieldId.ToString() + "&offset=" + offset.ToString() + "&limit=" + limit.ToString() + "&order=" + order;
            uriBuilder.Query = parameters;

            return client.GetAsync(uriBuilder.Uri).Result;
        }
    }
}