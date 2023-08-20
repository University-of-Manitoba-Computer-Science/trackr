import time
import requests

ApiEndpoint = "http://wryneck.cs.umanitoba.ca/api/values"

def AddSingleValue(apiKey, fieldId: int, value):
	"""
	Add's a single value to the field of the project which has the apikey.
	Usage:
        result = valueApi.AddSingleValue('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, 4678)
        print("status code: " + str(result.status_code))
        print("content: " + result.text)
	:param apiKey: The apiKey found in your Trackr account on the API page
	:param fieldId: The fieldId found in your Trackr account on the Fields page
	:param value: The value you wish to add to the field
	:return: The response which has 'status_code' and 'text'. Text will be empty if the status_code is successful, or will contain error message if not.
	"""
	if not isinstance(apiKey,str):
		apiKey = str(apiKey)
	if not isinstance(value,str):
		value = str(value)
	DATA = {
		'apiKey': apiKey,
		'value': value,
		'fieldId': fieldId
	}
	return requests.post(url=ApiEndpoint, data=DATA)

def AddManyValues(apiKey, fieldId: int, values):
	"""
	Add's a list of string values to be added to the field of the project which has the apiKey. Requires 1 second per value due to rate-limit.
	Usage:
		myValues = [
        "723",
        "2345"
    	]
        result = valueApi.AddManyValues('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, myValues)
        print("status code: " + str(result.status_code))
        print("content: " + result.text)
	:param apiKey: The apiKey found in your Trackr account on the API page
	:param fieldId: The fieldId found in your Trackr account on the Fields page
	:param values: A list of the values you wish to add to the field
	:return: The response which has 'status_code' and 'text'. Text will be empty if the status_code is successful, or will contain error message if not.
	"""
	result = 0
	for value in values:
		result = AddSingleValue(apiKey, fieldId, value)
		if result.status_code != 200:
			return result
		time.sleep(1)
	return result


def GetValues(apiKey, fieldId: int, offset: int, limit: int, order):
	"""
	Accepts configuration parameters which target a project which has the apiKey, to read in "limit" number of values from the field with "fieldId", starting at the "offset" value. Acceptable orders are "asc" or "desc".
	Usage:
		result = valueApi.GetValues('pVUYgaZgSabpgiSvjQsLsHv0nhj2wfsdG7j3k2nm7n2lynb6n3225hn234nhAQh0', 5, 3, 25, 'asc');
		print("status code: " + str(result.status_code))
		print("content: " + result.text) # could also use as json after importing json: dataJson = result.text.json()
	:param apiKey: The apiKey found in your Trackr account on the API page
	:param fieldId: The fieldId found in your Trackr account on the Fields page
	:param offset: The number of value's you would like to skip before starting to return values.
	:param limit: The number of value's you would like returned.
	:param order: The sorting applied to the returned values. Accepts 'asc' or 'desc'.
	:return: a tuple with 'values' and 'totalValues'. Values is a dictionary of tuples which are of the format {"id", "value", "createdAt"}
	"""
	if not isinstance(apiKey,str):
		apiKey = str(apiKey)
	if not isinstance(order,str):
		order = str(order)
	if order.lower() != "asc" and order.lower() != "desc":
		return "Order must be 'asc' or 'desc'. Please try again.", 400

	PARAMS = {
		'apiKey': apiKey,
		'fieldId': fieldId,
		'offset': offset,
		'limit': limit,
		'order': order
	}

	response = requests.get(url=ApiEndpoint, params=PARAMS)

	return response