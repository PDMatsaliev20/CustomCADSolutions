import { useState, useEffect } from 'react'
import { IsAuthenticated } from '@/requests/public/identity';
import { getCookie } from '@/utils/cookie-manager'

function useAuth() {
    const [username, setUsername] = useState(getCookie('username'));
    const [userRole, setUserRole] = useState(getCookie('role'));
    const [isAuthenticated, setIsAuthenticated] = useState(getCookie('username') && getCookie('role'));

    useEffect(() => {
        checkUserAuthentication();
        if (isAuthenticated) {
            setUsername(getCookie('username'));
            setUserRole(getCookie('role'));
        } else {
            setUsername('');
            setUserRole('');
        }
    }, [isAuthenticated]);

    return { username, userRole, isAuthenticated, setIsAuthenticated };
    
    async function checkUserAuthentication() {
        try {
            const { data } = await IsAuthenticated();

            if (data) {
                setIsAuthenticated(true);
            } else {
                setIsAuthenticated(false);
            }
        }
        catch (e) {
            console.error(e);
        }
    }
}

export default useAuth;