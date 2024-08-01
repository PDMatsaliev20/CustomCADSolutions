import AuthGuard from '../auth-guard'
import UserOrdersPage from '@/private/orders/user-orders/user-orders'
import OrderDetailsPage from '@/private/orders/order-details/order-details'
import CustomOrderPage from '@/private/orders/custom-order/custom-order'
import FinishedOrdersPage from '@/private/orders/finished-orders/finished-orders'
import { GetOrders, GetCompletedOrders, GetOrder } from '@/requests/private/orders'
import { GetCategories } from '@/requests/public/home'

export default {
    element: <AuthGuard isPrivate role={['Client']} />,
    children: [
        {
            path: '/orders',
            element: <UserOrdersPage />,
            loader: async () => {
                try {
                    const { data } = await GetOrders();
                    return { loadedOrders: data };
                } catch (e) {
                    console.error(e);
                }
            },
        },
        {
            path: '/orders/finished',
            element: <FinishedOrdersPage />,
            loader: async () => {
                try {
                    const { data } = await GetCompletedOrders();
                    return { loadedOrders: data };
                } catch (e) {
                    console.error(e);
                }
            }
        },
        {
            path: '/orders/:id',
            element: <OrderDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categories = await GetCategories();
                    const orderRes = await GetOrder(id);

                    return { loadedCategories: categories.data, loadedOrder: orderRes.data };
                } catch (e) {
                    console.error(e);
                }

            }
        },
        {
            path: '/orders/custom',
            element: <CustomOrderPage />
        },
    ]
};