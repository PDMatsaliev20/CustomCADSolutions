import AuthContext from '@/components/auth-context'
import { useNavigate } from 'react-router-dom'
import { useContext } from 'react'
import axios from 'axios'
import Router from '@/components/router'

function Body() {
    const { setIsAuthenticated } = useContext(AuthContext);
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
        <main className="basis-full mx-16 my-5">
            <Router onLogin={login} onRegister={register} />
        </main>                                                     
    );
}

export default Body;