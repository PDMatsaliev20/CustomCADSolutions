import App from '@/app'
import publicRoutes from './users/public'
import privateRoutes from './users/private'
import publicOnlyRoutes from './users/public-only'

const userRoutes = {
    path: '/',
    element: <App />,
    children: [
        publicRoutes,
        privateRoutes,
        publicOnlyRoutes
    ]
};

export default [userRoutes];