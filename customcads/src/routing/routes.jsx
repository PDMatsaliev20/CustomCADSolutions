import App from '@/app'
import publicRoutes from './users/public'
import guestRoutes from './users/guest'
import clientRoutes from './users/client'
import contributorRoutes from './users/contributor'
import designerRoutes from './users/designer'

const userRoutes = {
    path: '/',
    element: <App />,
    children: [
        publicRoutes,
        guestRoutes,
        clientRoutes,
        contributorRoutes,
        designerRoutes,
    ]
};

export default [userRoutes];