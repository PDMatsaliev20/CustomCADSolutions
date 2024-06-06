import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import Router from '../router'

function Body({ isAuthenticated, setIsAuthenticated }) {
    const navigate = useNavigate();

    const register = async (user, userRole) => {
        await axios.post(`https://localhost:7127/API/Identity/Register/${userRole}`, user, {
            withCredentials: true
        });

        localStorage.setItem('username', user.username);
        setIsAuthenticated(true);
        navigate("/");
    };

    const login = async (user) => {
        try {
            await axios.post(`https://localhost:7127/API/Identity/Login`, user, {
                withCredentials: true
            });

            localStorage.setItem('username', user.username);
            setIsAuthenticated(true);
            navigate("/");
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <main className="mx-16 pb-28">
            <Router onLogin={login} onRegister={register} isAuthenticated={isAuthenticated} />
        </main>                                                     
    );
}

export default Body;