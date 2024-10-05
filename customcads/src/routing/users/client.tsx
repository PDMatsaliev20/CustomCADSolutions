import { AxiosError } from 'axios';
import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/categories';
import { GetRecentOrders, GetOrder, GetOrdersCounts } from '@/requests/private/orders';
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
            element: <ClientHomePage />,
            loader: async () => {
                try {
                    const { data: orders } = await GetRecentOrders();
                    const { data: loadedCounts } = await GetOrdersCounts();
                    return { loadedOrders: orders, loadedCounts };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
                }
            }
        },
        {
            path: '/client/orders/:status',
            element: <UserOrdersPage />,
        },
        {
            path: '/client/orders/:status/:id',
            element: <OrderDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categories = await GetCategories();
                    const orderRes = await GetOrder(Number(id));
                    return { id: Number(id), loadedCategories: categories.data, loadedOrder: orderRes.data };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
                }
            }
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