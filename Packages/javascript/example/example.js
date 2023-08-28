const trackr = require('umtrackr')

const ValuesAPI = new trackr('rLdJdByBo0hBIOTM5CSXToxhPxkGgriHjWq8kGgAhyem5Lrncf28qu4pai9s6oCC')

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