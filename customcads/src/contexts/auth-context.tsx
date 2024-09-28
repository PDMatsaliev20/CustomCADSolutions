import { createContext, useState, useEffect, ReactNode, Dispatch, SetStateAction } from 'react';
import { IsAuthenticated } from '@/requests/public/identity';
import { getCookie } from '@/utils/cookie-manager';

interface AuthContextValues {
    username: string | null
    userRole: string | null
    isAuthenticated: boolean | null
    isLoading: boolean
    setIsAuthenticated: Dispatch<SetStateAction<boolean | null>>
}

const defaultValues: AuthContextValues = { username: '', userRole: '', isAuthenticated: false, isLoading: true, setIsAuthenticated: () => { } };
export const AuthContext = createContext(defaultValues);

interface AuthProviderProps {
    children: ReactNode
}
export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [username, setUsername] = useState<string | null>(getCookie('username') ?? null);
    const [userRole, setUserRole] = useState<string | null>(getCookie('role') ?? null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

    useEffect(() => {
        checkUserAuthentication();
    }, []);

    useEffect(() => {
        if (isAuthenticated) {
            setUsername(getCookie('username') ?? '');
            setUserRole(getCookie('role') ?? '');
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
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <AuthContext.Provider value={{ username, userRole, isLoading, isAuthenticated, setIsAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};