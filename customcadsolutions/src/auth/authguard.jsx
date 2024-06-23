import AuthContext from '../auth-context'
import { Outlet, Navigate } from 'react-router-dom'
import { useContext } from 'react'

function AuthGuard({ isPrivate }) {
    const { isAuthenticated } = useContext(AuthContext);
    
    if (!isAuthenticated && isPrivate) {
        return <Navigate to="/login" replace />;
    }

    return <Outlet />;
}

export default AuthGuard;