import { RouteObject } from 'react-router-dom';
import { AuthProvider } from '@/contexts/auth-context';
import ErrorPage from '@/components/error-page';
import App from '@/app';
import publicRoutes from './users/public';
import guestRoutes from './users/guest';
import clientRoutes from './users/client';
import contributorRoutes from './users/contributor';
import designerRoutes from './users/designer';

const userRoutes: RouteObject = {
    path: '/',
    element: <AuthProvider><App /></AuthProvider>,
    children: [
        publicRoutes,
        guestRoutes,
        clientRoutes,
        contributorRoutes,
        designerRoutes,
        { path: '*', element: <ErrorPage status={404} /> },
    ]
};

export default [userRoutes];