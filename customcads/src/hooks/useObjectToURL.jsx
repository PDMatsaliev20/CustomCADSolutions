export default (search) => {    
    return Object.entries(search).map(([key, value]) =>
        value && `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
    ).filter(q => q).join('&');
};