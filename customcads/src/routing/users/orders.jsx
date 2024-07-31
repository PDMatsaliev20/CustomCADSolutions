import AuthGuard from '../auth-guard'
import UserOrdersPage from '@/private/orders/user-orders/user-orders'
import OrderDetailsPage from '@/private/orders/order-details/order-details'
import CustomOrderPage from '@/private/orders/custom-order/custom-order'
import FinishedOrdersPage from '@/private/orders/finished-orders/finished-orders'
import axios from 'axios'

export default {
    element: <AuthGuard isPrivate role={['Client']} />,
    children: [
        {
            path: '/orders',
            element: <UserOrdersPage />,
            loader: async () => {
                const orders = await axios.get('https://localhost:7127/API/Orders', {
                    withCredentials: true
                }).catch(e => console.error(e));
                return { loadedOrders: orders.data };
            },
        },
        {
            path: '/orders/finished',
            element: <FinishedOrdersPage />,
            loader: async () => {
                const orders = await axios.get('https://localhost:7127/API/Orders/Completed', {
                    withCredentials: true
                }).catch(e => console.error(e));

                return { loadedOrders: orders.data };
            }
        },
        {
            path: '/orders/:id',
            element: <OrderDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                const categories = await axios.get('https://localhost:7127/API/Common/Categories', {
                    withCredentials: true
                }).catch(e => console.error(e));

                const order = await axios.get(`https://localhost:7127/API/Orders/${id}`, {
                    withCredentials: true
                }).catch(e => console.error(e));

                return { loadedCategories: categories.data, loadedOrder: order.data };
            }
        },
        {
            path: '/orders/custom',
            element: <CustomOrderPage />
        },
    ]
};