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
    const { i18n } = useTranslation();
    const hasUsername = localStorage.getItem('username');
    const [isAuthenticated, setIsAuthenticated] = useState(hasUsername);
    const [userRole, setUserRole] = useState();

    useEffect(() => {
        const language = localStorage.getItem('language');
        if (language && i18n.language !== language) {
            i18n.changeLanguage(language);
    }
    }, []);

    useEffect(() => {
        checkUserAuthentication();
        if (isAuthenticated) {
            checkUserAuthorization();
        }
    }, [isAuthenticated]);

    return (
        <BrowserRouter>
            <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated, userRole }}>
                <div className="flex flex-wrap items-start min-h-screen bg-indigo-50">
                    <div className="basis-full grow sticky top-0 z-50">
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