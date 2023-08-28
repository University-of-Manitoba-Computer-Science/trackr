# Trackr

Clientside library to get or add values to Trackr server. See GitHub project for more details.

## Examples

Example nodejs script to push/pull data:

```js
const trackr = require('umtrackr')

const ValuesAPI = new trackr('[api key]')

async function main() {
    let res;

    for (i = 1; i < 6; i++) {
        res = await ValuesAPI.addValue(1, i)
        console.log(res)
    }

    res = await ValuesAPI.addValues(1, [6, 7, 8, 9, 10])
    console.log(res)

    res = await ValuesAPI.getValues(1, 0, 10, 'desc')
    console.log(res)
}

main()
```