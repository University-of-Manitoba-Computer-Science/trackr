import json
import requests

ApiEndpoint = "http://wryneck.cs.umanitoba.ca:3000/values"

def addSingleValue(apiKey, fieldId, value):
	PARAMS = {
		'ApiKey':apiKey,
		'Value':fieldId,
		'FieldId':value
	}
	jsonString = json.dumps(PARAMS)
	request = requests.post(url = URL, params = params)

	if request.status_code == 200:
		return True
	else:
		return False

def addManyValues(apiKey, fieldId, values):
	for value in values:
		bool result = addSingleValue(apiKey, fieldId, value)
		if !result:
			return result
	return True


def getValues(apiKey, fieldId, offset, limit, order):
	if order.lower() != "asc" and order.lower() != "desc":
		print("order must be 'asc' or 'desc'. Please try again.")
		return False

	PARAMS = {
		'ApiKey':apiKey,
		'Value':fieldId,
		'Offset':offset,
		'Limit':limit,
		'Order':order
	}
	
	jsonString = json.dumps(PARAMS)
	request = requests.get(url = URL, params = params)

	if request.status_code == 200:
		return True
	else:
		return False