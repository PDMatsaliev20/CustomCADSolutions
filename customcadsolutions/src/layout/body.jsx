import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import Router from '../router'

function Body({ setIsAuthenticated }) {
    const navigate = useNavigate();

    const register = async (user, userRole) => {
        const { token, role, username } = await axios.post(`https://localhost:7127/API/Identity/Register/${userRole}`, user)
            .then(response => response.data);

        localStorage.setItem('token', token);
        localStorage.setItem('username', username);
        localStorage.setItem('role', role);

        setIsAuthenticated(true);

        navigate("/");
    };

    const login = async (user) => {
        const { token, role, username } = await axios.post(`https://localhost:7127/API/Identity/Login`, user)
            .then(response => response.data);

        localStorage.setItem('token', token);
        localStorage.setItem('username', username);
        localStorage.setItem('role', role);

        setIsAuthenticated(true);

        navigate("/");
    };

    const isInRole = (role) => localStorage.getItem('role') === role;

    return (
        <main className="mx-16 pb-28">
            <Router onLogin={login} onRegister={register} isAuthenticated={isAuthenticated} />
        </main>
    );
}

export default Body;