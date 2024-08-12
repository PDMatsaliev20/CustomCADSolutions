import { useState, useEffect, useContext } from 'react';
import { Outlet } from 'react-router-dom';
import AuthContext from '@/contexts/auth-context';
import ErrorPage from '@/components/error-page';

function AuthGuard({ auth, roles }) {
    const { isAuthenticated, userRole } = useContext(AuthContext);
    const [response, setResponse] = useState(<Outlet />);
    
    useEffect(() => {
        if (auth === 'guest' && isAuthenticated) {
            setResponse(<ErrorPage status={400} />);
        } else if (auth === 'private') {
            if (!isAuthenticated) {
                setResponse(<ErrorPage status={401} />);
            } else if (roles && userRole && !roles.includes(userRole)) {
                setResponse(<ErrorPage status={403} />);
            }
        } else {
            setResponse(<Outlet />);
        }
    }, [auth, roles, isAuthenticated, userRole]);

    return response;
}

export default AuthGuard;