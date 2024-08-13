import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetRecentOrders, GetOrder, GetOrdersCounts } from '@/requests/private/orders';
import ClientHomePage from '@/pages/client/client-home/client-home';
import UserOrdersPage from '@/pages/client/user-orders/orders';
import OrderDetailsPage from '@/pages/client/order-details/order-details';
import PurchasePage from '@/pages/client/purchase/purchase';
import CustomOrderPage from '@/pages/client/custom-order/custom-order';
import capitalize from '@/utils/capitalize';

export default {
    path: '/client',
    element: <AuthGuard auth="private" role="Client" />,
    children: [
        {
            path: '',
            element: <ClientHomePage />,
            loader: async () => {
                const { data: { orders } } = await GetRecentOrders();
                const { data: loadedCounts } = await GetOrdersCounts();
                return { loadedOrders: orders, loadedCounts };
            }
        },
        {
            path: 'orders/:status',
            element: <UserOrdersPage />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
        },
        {
            path: 'orders/:status/:id',
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
            path: 'orders/custom',
            element: <CustomOrderPage />
        },
        {
            path: 'purchase/:id',
            element: <PurchasePage />
        },
    ]
};