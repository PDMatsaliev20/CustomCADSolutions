import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import ClientHomePage from '@/pages/client/client-home/client-home';
import UserOrdersPage from '@/pages/client/user-orders/orders';
import OrderDetailsPage from '@/pages/client/order-details/order-details';
import PurchasePage from '@/pages/client/purchase/purchase';
import CustomOrderPage from '@/pages/client/custom-order/custom-order';

const clientRoutes: RouteObject = {
    element: <AuthGuard auth="private" role="Client" />,
    children: [
        {
            path: '/client',
            element: <ClientHomePage />
        },
        {
            path: '/client/orders/:status',
            element: <UserOrdersPage />,
        },
        {
            path: '/client/orders/:status/:id',
            element: <OrderDetailsPage />
        },
        {
            path: '/client/orders/custom',
            element: <CustomOrderPage />
        },
        {
            path: '/client/purchase/:id',
            element: <PurchasePage />
        },
    ]
};
export default clientRoutes;