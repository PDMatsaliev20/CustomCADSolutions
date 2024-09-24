import { createContext, useState, useEffect, ReactNode,  Dispatch, SetStateAction } from 'react';
import { IsAuthenticated } from '@/requests/public/identity';
import { getCookie } from '@/utils/cookie-manager';

interface AuthContextValues {
    username: string | null
    userRole: string | null
    isAuthenticated: boolean
    setIsAuthenticated: Dispatch<SetStateAction<boolean>>
}

const defaultValues: AuthContextValues = { username: '', userRole: '', isAuthenticated: false, setIsAuthenticated: () => {} };
export const AuthContext = createContext(defaultValues);

interface AuthProviderProps {
    children: ReactNode
}
export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [username, setUsername] = useState<string | null>(getCookie('username') ?? null);
    const [userRole, setUserRole] = useState<string | null>(getCookie('role') ?? null);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(true);

    useEffect(() => {
        checkUserAuthentication();
    }, []);
    
    useEffect(() => {
        if (isAuthenticated) {
            setUsername(getCookie('username') ?? '');
            setUserRole(getCookie('role') ?? '');
        } else {
            setUsername('');
            setUserRole('');
        }
    }, [isAuthenticated]);

    async function checkUserAuthentication() {
        try {
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