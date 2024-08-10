function getCookie(cookieName) {
    const cookies = document.cookie.split('; ');
    const usernameCookie = cookies.find(cookie => cookie.split('=')[0] === cookieName);
    const username = usernameCookie && usernameCookie.split('=')[1];
    return username;
};

function setCookie(cookieName, cookieValue) {
    document.cookie = `${cookieName}=${cookieValue}`;
};

export { getCookie, setCookie };