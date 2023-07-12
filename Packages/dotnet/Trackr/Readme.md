# Instructions
### Step 1:
From nuget package manager, install Trackr package
## Step 2:
add using statement to project
## Step 3:
Use Trackr functions within your own code. 



# Example working cs file
using Trackr;

string myApiKey = "pVUYgxZySwbp6iSvmQQLQHl0ywA2X3m5Gg93cKSFoMPU5k6IVTWgoUUV9YpsAQh0";

uint myFieldId = 1;

uint myOffset = 0;

int myLimit = 10;

string myOrder = "asc";

string myUrl = "someAddress/api/values";

List\<string> myValues = new List\<string>
{
    "1",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7"
};

<br/><br/>

await testManyValues(myApiKey, myFieldId, myValues);

await testSingleValue(myApiKey, myFieldId, myValues[0]);

await testGetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder);

testUpdateEndpoint(myUrl);

<br/><br/>

static void testUpdateEndpoint(string url)

{<br/><br/>
    Console.WriteLine("Current endpoint is: " + Trackr.Trackr.ShowEndpoint());<br/>
    Trackr.Trackr.UpdateEndpoint(url);<br/>
    Console.WriteLine("New endpoint is: " + Trackr.Trackr.ShowEndpoint());<br/>
}

static async Task testManyValues(string myApiKey, uint myFieldId, List\<string> myValues)<br/>
{<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Adding many values");<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HttpResponseMessage response = await Trackr.Trackr.AddManyValues(myApiKey, myFieldId, myValues);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Content: " + response.Content);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Status: " + response.StatusCode);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Done adding many values");<br/>
}

static async Task testSingleValue(string myApiKey, uint myFieldId, string myValue)<br/>
{<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Adding a value");<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HttpResponseMessage response = await Trackr.Trackr.AddSingleValue(myApiKey, myFieldId, myValue);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Content: " + response.Content);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Status: " + response.StatusCode);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Done adding one value");<br/>
}

static async Task testGetValues(string myApiKey, uint myFieldId, uint myOffset, int myLimit, string myOrder)<br/>
{<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Getting values");<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HttpResponseMessage response = await Trackr.Trackr.GetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Content: " + response.Content);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Status: " + response.StatusCode);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if (response.IsSuccessStatusCode)<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{<br/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;string responseBody = await response.Content.ReadAsStringAsync();<br/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine(responseBody);<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Console.WriteLine("Done getting values");<br/>
}<br/>