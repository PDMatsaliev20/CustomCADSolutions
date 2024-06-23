import AuthContext from './auth-context'
import Header from './layout/header'
import Navbar from './layout/navbar'
import Body from './layout/body'
import Footer from './layout/footer'
import { BrowserRouter } from 'react-router-dom'
import { useState, useEffect } from 'react'
import axios from 'axios'
import './index.css'

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userRole, setUserRole] = useState();

    useEffect(() => {
        checkIfAuthenticated();
        if (isAuthenticated) {
            getUserRole();
        }
    }, [isAuthenticated]);

    return (
        <div className="relative min-h-screen bg-indigo-50">
            <BrowserRouter>
                <AuthContext.Provider value={{isAuthenticated, setIsAuthenticated, userRole }}>
                    <Header />
                    <Navbar />
                    <Body />
                    <Footer />
                </AuthContext.Provider>
            </BrowserRouter>
        </div>
    );

    async function checkIfAuthenticated() {
        const response = await axios.get('https://localhost:7127/API/Identity/IsAuthenticated', {
            withCredentials: true
        }).catch(e => console.error(e));
        if (response.data) {
            setIsAuthenticated(true);
        }
    }

    async function getUserRole() {
        const response = await axios.get('https://localhost:7127/API/Identity/GetUserRole', {
            withCredentials: true
        }).catch(e => console.error(e));
        const role = response.data;
        setUserRole(role);
    }
}

export default App;