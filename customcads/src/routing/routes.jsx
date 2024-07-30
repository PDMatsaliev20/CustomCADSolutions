import App from '@/app'
import publicRoutes from './users/public'
import guestRoutes from './users/guest'
import clientRoutes from './users/orders'
import contributorRoutes from './users/cads'
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