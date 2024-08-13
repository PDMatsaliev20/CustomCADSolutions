import ErrorPage from '@/components/error-page';
import App from '@/app';
import publicRoutes from './users/public';
import guestRoutes from './users/guest';
import clientRoutes from './users/private/client';
import contributorRoutes from './users/private/contributor';
import designerRoutes from './users/private/designer';

const userRoutes = {
    path: '/',
    element: <App />,
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