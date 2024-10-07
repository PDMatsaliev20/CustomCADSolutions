import { RouteObject } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from '@/contexts/auth-context';
import ErrorPage from '@/components/error-page';
import App from '@/app';
import publicRoutes from './routes/public';
import guestRoutes from './routes/guest';
import clientRoutes from './routes/client';
import contributorRoutes from './routes/contributor';
import designerRoutes from './routes/designer';

const userRoutes: RouteObject = {
    path: '/',
    element:
        <AuthProvider>
            <QueryClientProvider client={new QueryClient()}>
                <App />
            </QueryClientProvider>
        </AuthProvider>,
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