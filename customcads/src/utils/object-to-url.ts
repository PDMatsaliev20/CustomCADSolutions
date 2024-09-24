export default (search: object): string => {    
    return Object.entries(search).map(([key, value]) =>
        value && `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
    ).filter(q => q).join('&');
};