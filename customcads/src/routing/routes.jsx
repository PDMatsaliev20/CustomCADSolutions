import App from '@/app'
import publicRoutes from './users/public'
import guestRoutes from './users/guest'
import clientRoutes from './users/private/orders'
import contributorRoutes from './users/private/cads'
import designerRoutes from './users/private/designer'

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