export default (query) => {

    // [ [key: searchName, value: ''], [key: searchCreator, value: ''], [key: category, value: '']... ]
    const queryKeyValueArrays = Object.entries(query);
    
    // ['', '', '', 'sorting=1', 'validated=true'...]
    const queryStringArray = queryKeyValueArrays.map(
        ([key, value]) =>
            (key.includes('validated') || value)
                ? `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
                : ''
    );

    // ['sorting=1', 'validated=true', 'unvalidated=false'...]
    const queryStringArrayWithoutEmptyEntries = queryStringArray.filter(q => q || (typeof q == Boolean));

    // 'sorting=1&validated=true&unvalidated=false...'
    const queryString = queryStringArrayWithoutEmptyEntries.join('&');

    return queryString;
};