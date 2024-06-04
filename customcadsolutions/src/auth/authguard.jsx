import { Outlet, Navigate } from 'react-router-dom'

function AuthGuard({ isPrivate, isAuthenticated }) {
    if (!isAuthenticated && isPrivate) {
        return <Navigate to="/login" replace />;
    }

    return <Outlet />;
}

export default AuthGuard;