import { useState, useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import AuthContext from '@/components/auth-context';
import { IsAuthenticated, GetUserRole } from '@/requests/public/identity';
import Header from '@/layout/header/header';
import Navbar from '@/layout/navbar/navbar';
import Footer from '@/layout/footer/footer';
import './index.css';

function App() {
    const { i18n } = useTranslation();

    const getCookie = (cookieName) => {
        const cookies = document.cookie.split('; ');
        const usernameCookie = cookies.find(cookie => cookie.split('=')[0] === cookieName);
        return usernameCookie && usernameCookie.split('=')[1];
    };

    const [isAuthenticated, setIsAuthenticated] = useState(getCookie('username'));
    const [userRole, setUserRole] = useState();
    const [username, setUsername] = useState();

    useEffect(() => {
        const language = localStorage.getItem('language');
        if (language && i18n.language !== language) {
            i18n.changeLanguage(language);
            localStorage.setItem('language', language);
        }
    }, []);

    useEffect(() => {
        checkUserAuthentication();
        if (isAuthenticated) {
            setUsername(getCookie('username'));
            checkUserAuthorization();
        }
    }, [isAuthenticated]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated, username, userRole }}>
            <div className="flex flex-col min-h-screen bg-indigo-50">
                <div className="sticky top-0 z-50">
                    <Header />
                    <Navbar />
                </div>
                <main className="basis-full grow self-stretch my-8 mx-16">
                    <Outlet />
                </main>
                <Footer />
            </div>
        </AuthContext.Provider>
    );

    async function checkUserAuthentication() {
        try {
            const { data } = await IsAuthenticated();

            if (data) {
                setIsAuthenticated(true);
            } else {
                setIsAuthenticated(false);
            }
        }
        catch (e) {
            console.error(e);
        }
    }

    async function checkUserAuthorization() {
        try {
            const { data } = await GetUserRole();
            setUserRole(data);
        }
        catch (e) {
            console.error(e);
        }
    }
}

export default App;