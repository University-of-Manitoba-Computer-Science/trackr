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
        /// Allows the user to see the currently set endpoint
        /// </summary>
        /// <returns>A string which is the current endpoint</returns>
        public static string ShowEndpoint()
        {
            return ApiEndpoint;
        }

        /// <summary>
        /// Updates the api endpoint to be used. This function is used when hosting a local version of Trackr and therefore needs a different endpoint than the one hosted at the University of Manitoba.
        /// Usage:
        /// valueApi.UpdateEndpoint('someAddress/api/values')
        /// </summary>
        /// <param name="url">The url where the locally hosted version of Trackr is found.</param>
        /// <returns>None</returns>
        public static void UpdateEndpoint(string url)
        {
            ApiEndpoint = url;
        }

        /// <summary>
        /// Add's a single value to the field of the project which has the apikey.
        /// Usage:
        ///   using Trackr;
        ///   HttpResponseMessage response = await TrackrApi.AddManyValues('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, 12561);
        ///   Console.WriteLine("Content: " + response.Content);
        ///   Console.WriteLine("Status: " + response.StatusCode);
        /// </summary>
        /// <param name="apiKey">The apiKey found in your Trackr account on the API page</param>
        /// <param name="fieldId">The fieldId found in your Trackr account on the Fields page</param>
        /// <param name="value">The value you wish to add to the field</param>
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
        /// Usage:
        ///   using Trackr;
        ///   List<string> myValues = new List<string>
        ///   {
        ///      "1",
        ///      "2"
        ///   }
        ///   HttpResponseMessage response = await TrackrApi.AddManyValues('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, myValues);
        ///   Console.WriteLine("Content: " + response.Content);
        ///   Console.WriteLine("Status: " + response.StatusCode);
        /// </summary>
        /// <param name="apiKey">The apiKey found in your Trackr account on the API page</param>
        /// <param name="fieldId">The fieldId found in your Trackr account on the Fields page</param>
        /// <param name="values">A list of the values you wish to add to the field</param>
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
        /// Usage:
        ///     HttpResponseMessage response = await TrackrApi.GetValues('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, 4, 25, 'desc');
        ///     Console.WriteLine("Content: " + response.Content);
        ///     Console.WriteLine("Status: " + response.StatusCode);
        ///     if (response.IsSuccessStatusCode)
        ///     {
        ///        string responseBody = await response.Content.ReadAsStringAsync();
        ///        Console.WriteLine(responseBody);
        ///     }
        /// </summary>
        /// <param name="apiKey">The apiKey found in your Trackr account on the API page</param>
        /// <param name="fieldId">The fieldId found in your Trackr account on the Fields page</param>
        /// <param name="offset">The number of value's you would like to skip before starting to return values.</param>
        /// <param name="limit">The number of value's you would like returned.</param>
        /// <param name="order">The sorting applied to the returned values. Accepts 'asc' or 'desc'.</param>
        /// <returns>Returns an HttpResponseMessage with a statusCode and Content detailing the result of the operation. If the operation was successful, the content will contain "totalValues" which is the current number of values for the field and "values" which is a list of the values returned. Result can be read with 'response.Content.ReadAsStringAsync()' and then parsed.</returns>
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