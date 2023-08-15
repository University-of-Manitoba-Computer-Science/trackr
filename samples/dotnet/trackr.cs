using Trackr;

string myApiKey = "pVUYgxZySwbp6iSvmQQLQHl0ywA2X3m5Gg93cKSFoMPU5k6IVTWgoUUV9YpsAQh0";
uint myFieldId = 1;
uint myOffset = 0;
int myLimit = 10;
string myOrder = "asc";
string myUrl = "someAddress/api/values";

List<string> myValues = new List<string>
{
    "1",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7"
};

await testManyValues(myApiKey, myFieldId, myValues);
Console.WriteLine();
await testSingleValue(myApiKey, myFieldId, myValues[0]);
Console.WriteLine();
await testGetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder);
Console.WriteLine();
testUpdateEndpoint(myUrl);

static void testUpdateEndpoint(string url)
{
    Console.WriteLine("Current endpoint is: " + Trackr.Trackr.ShowEndpoint());
    Trackr.Trackr.UpdateEndpoint(url);
    Console.WriteLine("New endpoint is: " + Trackr.Trackr.ShowEndpoint());
}

static async Task testManyValues(string myApiKey, uint myFieldId, List<string> myValues)
{
    Console.WriteLine("Adding many values");

    HttpResponseMessage response = await Trackr.Trackr.AddManyValues(myApiKey, myFieldId, myValues);

    Console.WriteLine("Content: " + response.Content);
    Console.WriteLine("Status: " + response.StatusCode);

    Console.WriteLine("Done adding many values");
}

static async Task testSingleValue(string myApiKey, uint myFieldId, string myValue)
{
    Console.WriteLine("Adding a value");

    HttpResponseMessage response = await Trackr.Trackr.AddSingleValue(myApiKey, myFieldId, myValue);

    Console.WriteLine("Content: " + response.Content);
    Console.WriteLine("Status: " + response.StatusCode);

    Console.WriteLine("Done adding one value");
}

static async Task testGetValues(string myApiKey, uint myFieldId, uint myOffset, int myLimit, string myOrder)
{
    Console.WriteLine("Getting values");

    HttpResponseMessage response = await Trackr.Trackr.GetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder);


    Console.WriteLine("Content: " + response.Content);
    Console.WriteLine("Status: " + response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
    Console.WriteLine("Done getting values");
}