import AuthContext from '@/components/auth-context'
import Header from './layout/header/header'
import Navbar from './layout/navbar/navbar'
import Body from './layout/body'
import Footer from './layout/footer'
import { BrowserRouter } from 'react-router-dom'
import { useState, useEffect } from 'react'
import { useTranslation } from 'react-i18next'
import axios from 'axios'
import './index.css'
import { library } from '@fortawesome/fontawesome-svg-core'
import { fas } from '@fortawesome/free-solid-svg-icons'

library.add(fas);

function App() {
    const { t, i18n } = useTranslation();

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

    const handleRegister = async (user, userRole) => {
        await axios.post(`https://localhost:7127/API/Identity/Register/${userRole}`, user, {
            withCredentials: true
        });

        setIsAuthenticated(true);
    };

    const handleLogin = async (user) => {
        try {
            await axios.post(`https://localhost:7127/API/Identity/Login`, user, {
                withCredentials: true
            });

            setIsAuthenticated(true);
        } catch (e) {
            return t('common.errors.Non-existent account or wrong password');
        }
    };

    const handleLogout = async () => {
        await axios.post('https://localhost:7127/API/Identity/Logout', {}, {
            withCredentials: true
        });

        setIsAuthenticated(false);
    };

    const contextValues = {
        onRegister: handleRegister,
        onLogin: handleLogin,
        onLogout: handleLogout,
        isAuthenticated,
        setIsAuthenticated,
        username,
        userRole
    };

    return (
        <BrowserRouter>
            <AuthContext.Provider value={contextValues}>
                <div className="flex flex-col min-h-screen bg-indigo-50">
                    <div className="sticky top-0 z-50">
                        <Header />
                        <Navbar />
                    </div>
                    <Body />
                    <Footer />
                </div>
            </AuthContext.Provider>
        </BrowserRouter>
    );

    async function checkUserAuthentication() {
        const response = await axios.get(
            'https://localhost:7127/API/Identity/IsAuthenticated',
            { withCredentials: true }
        ).catch(e => console.error(e));

        if (response.data) {
            setIsAuthenticated(true);
        } else {
            setIsAuthenticated(false);
        }
    }

    async function checkUserAuthorization() {
        const response = await axios.get(
            'https://localhost:7127/API/Identity/GetUserRole',
            { withCredentials: true }
        ).catch(e => console.error(e));

        setUserRole(response.data);
    }
}

export default App;