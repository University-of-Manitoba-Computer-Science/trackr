import json
import requests

ApiEndpoint = "http://wryneck.cs.umanitoba.ca/api/values"

def addSingleValue(apiKey, fieldId: int, value):
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

def addManyValues(apiKey, fieldId: int, values):
	for value in values:
		if not isinstance(apiKey,str):
			apiKey = str(apiKey)
		if not isinstance(values,str):
			values = str(values)
		bool result = addSingleValue(apiKey, fieldId, value)
		if !result:
			return result
	return True


def getValues(apiKey, fieldId: int, offset: int, limit: int, order):
  if not isinstance(apiKey,str):
      apiKey = str(apiKey)
  if not isinstance(order,str):
      order = str(order)
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