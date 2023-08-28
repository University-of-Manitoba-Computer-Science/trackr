class ValuesAPI {
  constructor(apikey, apiurl) {
    this.apikey = apikey;
    this.apiurl = (apiurl ?? 'http://wryneck.cs.umanitoba.ca') + '/api/values/';
    this.lastRequest = 0;
  }

  async getValues(fieldId, offset, limit, order) {
    const delay = Date.now() - this.lastRequest;

    if (delay < 1000) {
      await new Promise(resolve => setTimeout(resolve, 1000 - delay));
    }

    console.log(this.apiurl + `?apiKey=${this.apiKey}&fieldId=${fieldId}&offset=${offset}&limit=${limit}&order=${order}`);

    const response = await fetch(this.apiurl + `?apiKey=${this.apikey}&fieldId=${fieldId}&offset=${offset}&limit=${limit}&order=${order}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    
    const result = await response.json();

    this.lastRequest = Date.now();

    return result;
  }

  async addValue(fieldId, value) {
    const delay = Date.now() - this.lastRequest;

    if (delay < 1000) {
      await new Promise(resolve => setTimeout(resolve, 1000 - delay));
    }

    const response = await fetch(this.apiurl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({ 
        apiKey: this.apikey,
        fieldId: fieldId,
        value: value
       }).toString()
    });

    const result = await response.json();

    this.lastRequest = Date.now();

    return result;
  }

  async addValues(fieldId, values) {
    const result = [];

    for (const value of values) {
      result.push(await this.addValue(fieldId, value));
    }

    return result;
  }
}

module.exports = ValuesAPI;
