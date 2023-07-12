import Trackr

def testUpdateEndpoint(myUrl):
    print("Current endpoint is " + Trackr.ShowEndpoint())

    Trackr.UpdateEndpoint(myUrl)

    print("New endpoint is " + Trackr.ShowEndpoint())

def testManyValues(myApiKey, myFieldId: int, values):
    print("adding many values")
    response = Trackr.AddManyValues(myApiKey, myFieldId, values)

    print("Returned status code: " + str(response.status_code))
    print("Returned content: " + str(response.text))

    print("done adding many values")

def testSingleValue(myApiKey, myFieldId: int, value):
    print("adding single value")
    response = Trackr.AddSingleValue(myApiKey, myFieldId, value)

    print("Returned status code: " + str(response.status_code))
    print("Returned content: " + str(response.text))

    print("done adding single value")

def testGetValues(myApiKey, myFieldId: int, myOffset: int, myLimit: int, myOrder):
    print("getting values")
    returnedValues = Trackr.GetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder)

    print(returnedValues)

    print("done getting values")

myApiKey = "pVUYgxZySwbp6iSvmQQLQHl0ywA2X3m5Gg93cKSFoMPU5k6IVTWgoUUV9YpsAQh0"
myFieldId = 1
myOffset = 0
myLimit = 10
myOrder = "asc"
myUrl = "someAddress/api/values"

myValues = [
    "1",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7"
]

testManyValues(myApiKey, myFieldId, myValues)
print("")
testSingleValue(myApiKey, myFieldId, myValues[0])
print("")
testGetValues(myApiKey, myFieldId, myOffset, myLimit, myOrder)
print("")
testUpdateEndpoint(myUrl)