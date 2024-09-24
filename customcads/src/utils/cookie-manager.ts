function getCookie(cookieName: string): string | undefined {
    const cookies = document.cookie.split('; ');
    const usernameCookie = cookies.find(cookie => cookie.split('=')[0] === cookieName);
    const username = usernameCookie && usernameCookie.split('=')[1];
    return username;
};

function setCookie(cookieName: string, cookieValue: string) {
    document.cookie = `${cookieName}=${cookieValue}`;
};

export { getCookie, setCookie };