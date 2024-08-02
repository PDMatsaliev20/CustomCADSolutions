import AuthGuard from '../auth-guard'
import UserOrdersPage from '@/private/orders/user-orders/orders'
import OrderDetailsPage from '@/private/orders/order-details/order-details'
import CustomOrderPage from '@/private/orders/custom-order/custom-order'
import { GetOrders, GetOrder } from '@/requests/private/orders'
import { GetCategories } from '@/requests/public/home'

export default {
    element: <AuthGuard isPrivate role={['Client']} />,
    children: [
        {
            path: '/orders/:status',
            element: <UserOrdersPage />,
            loader: async ({ params }) => {
                const { status } = params;
                try {
                    const { data: { orders } } = await GetOrders(status);
                    return { loadedOrders: orders };
                } catch (e) {
                    console.error(e);
                }
            },
        },
        {
            path: '/orders/:status/:id',
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