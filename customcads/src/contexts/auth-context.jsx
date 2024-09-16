import { createContext, useContext, useState, useEffect } from 'react';
import { IsAuthenticated, RefreshToken } from '@/requests/public/identity';
import { getCookie } from '@/utils/cookie-manager';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [username, setUsername] = useState(getCookie('username'));
    const [userRole, setUserRole] = useState(getCookie('role'));
    const [isAuthenticated, setIsAuthenticated] = useState(username && userRole);

    useEffect(() => {
        checkUserAuthentication();
    }, []);

    useEffect(() => {
        if (isAuthenticated) {
            setUsername(getCookie('username'));
            setUserRole(getCookie('role'));
        } else {
            setUsername('');
            setUserRole('');
        }
    }, [isAuthenticated]);

    async function checkUserAuthentication() {
        try {
            await RefreshToken();
            const { data } = await IsAuthenticated();
            if (data) {
                setIsAuthenticated(true);
            } else {
                setIsAuthenticated(false);
            }
        } catch (e) {
            console.error(e);
        }
    }

    return (
        <AuthContext.Provider value={{ username, userRole, isAuthenticated, setIsAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);