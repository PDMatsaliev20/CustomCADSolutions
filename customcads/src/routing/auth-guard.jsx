import AuthContext from '@/components/auth-context'
import { Outlet, Navigate } from 'react-router-dom'
import { useContext } from 'react'

function AuthGuard({ isPrivate, isGuest, role }) {
    const { isAuthenticated, userRole } = useContext(AuthContext);

    if (!isAuthenticated && isPrivate) {
        return <Navigate to="/login" replace />;
    }

    if (isAuthenticated) {
        if (isGuest) {
            return <Navigate to="/home" replace />;
        }

        if (isPrivate && userRole && userRole !== role) {
            return <Navigate to="/home" replace />;
        }
    }
    
    return <Outlet />;
}

export default AuthGuard;