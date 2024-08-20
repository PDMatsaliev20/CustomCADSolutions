import { useState, useEffect } from 'react';
import { Outlet, Navigate } from 'react-router-dom';
import { useAuth } from '@/contexts/auth-context';
import ErrorPage from '@/components/error-page';

function AuthGuard({ auth, role }) {
    const { isAuthenticated, userRole } = useAuth();
    const [response, setResponse] = useState(<Outlet />);
    
    useEffect(() => {        
        if (auth === 'guest' && isAuthenticated) {
            if (userRole) {
                setResponse(<Navigate to={`/${userRole.toLowerCase()}`} />);
            }
        } else if (auth === 'private') {
            if (!isAuthenticated) {
                setResponse(<ErrorPage status={401} />);
            } else if (userRole) {
                if (role !== userRole) {
                    setResponse(<ErrorPage status={403} />);
                } else {
                    setResponse(<Outlet />);
                }
            }
        } else {
            setResponse(<Outlet />);
        }
    }, [auth, role]);

    return response;
}

export default AuthGuard;