import AuthContext from '@/components/auth-context'
import { Outlet, Navigate } from 'react-router-dom'
import { useContext } from 'react'

function AuthGuard({ isPrivate, isPrivateOnly }) {
    const { isAuthenticated } = useContext(AuthContext);
    
    if (!isAuthenticated && isPrivate) {
        return <Navigate to="/login" replace />;
    }

    if (isAuthenticated && isPrivateOnly) {
        return <Navigate to="/home" replace />;
    }

    return <Outlet />;
}

export default AuthGuard;