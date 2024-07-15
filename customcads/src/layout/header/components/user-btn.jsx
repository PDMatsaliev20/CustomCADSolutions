import LoginBtn from './login-btn'
import NewAccountBtn from './account-btn'
import AuthContext from '@/components/auth-context'
import { useContext } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'

function UserBtn() {
    const navigate = useNavigate();
    const { isAuthenticated, setIsAuthenticated, username } = useContext(AuthContext);
    
    const logout = async () => {
        await axios.post('https://localhost:7127/API/Identity/Logout', {}, {
            withCredentials: true
        });

        setIsAuthenticated(false);
        navigate("/");
    };

    let button = <NewAccountBtn onLogout={logout} username={username} />;
    if (!isAuthenticated) {
        button = <LoginBtn />;
    }

    return button;
}

export default UserBtn;