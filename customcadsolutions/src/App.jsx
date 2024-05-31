import Header from './layout/header'
import Navbar from './layout/navbar'
import Body from './layout/body'
import Footer from './layout/footer'
import { BrowserRouter } from 'react-router-dom'
import { useState, useEffect } from 'react'
import './index.css'

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (typeof token === "string" && token.length > 0) {
            setIsAuthenticated(true);
        }
    }, [isAuthenticated]);

    return (
        <div className="relative min-h-screen bg-indigo-50">
            <BrowserRouter>
                <Header isAuthenticated={isAuthenticated} setIsAuthenticated={setIsAuthenticated} />
                <Navbar />
                <Body setIsAuthenticated={setIsAuthenticated} />
                <Footer />
            </BrowserRouter>
        </div>
    );
}

export default App;