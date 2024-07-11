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
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [exists, setExists] = useState(false);
    const [userRole, setUserRole] = useState();

    if (localStorage.getItem('language') && i18n.language !== localStorage.getItem('language')) {
        i18n.changeLanguage(localStorage.getItem('language'));
    }

    useEffect(() => {
        checkUserAuthentication();
        checkUserExists();
        if (exists && isAuthenticated) {
            checkUserAuthorization();
        }
    }, [isAuthenticated, exists]);

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

        if (!response.data) {
            setIsAuthenticated(false);
        } else if (exists) {
            setIsAuthenticated(true);
        }
    }

    async function checkUserExists() {
        const response = await axios.get(
            'https://localhost:7127/API/Identity/UserExists',
            { withCredentials: true }
        ).catch(e => console.error(e));

        if (response.data) {
            setExists(true);
        } else {
            setExists(false);
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