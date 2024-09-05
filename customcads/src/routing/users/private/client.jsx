import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetRecentOrders, GetOrder, GetOrdersCounts } from '@/requests/private/orders';
import ClientHomePage from '@/pages/client/client-home';
import UserOrdersPage from '@/pages/client/user-orders/orders';
import OrderDetailsPage from '@/pages/client/order-details/order-details';
import PurchasePage from '@/pages/client/purchase/purchase';
import CustomOrderPage from '@/pages/client/custom-order/custom-order';
import capitalize from '@/utils/capitalize';

export default {
    element: <AuthGuard auth="private" role="Client" />,
    children: [
        {
            path: '/client',
            element: <ClientHomePage />,
            loader: async () => {
                try {
                    const { data: { orders } } = await GetRecentOrders();
                    const { data: loadedCounts } = await GetOrdersCounts();

                    return { loadedOrders: orders, loadedCounts };
                } catch (e) {
                    console.error(e);
                    switch (e.response.status) {
                        case 401: return { error: true, unauthenticated: true }; break;
                        case 403: return { error: true, unauthorized: true }; break;
                        default: return { error: true }; break;
                    }
                }
            }
        },
        {
            path: '/client/orders/:status',
            element: <UserOrdersPage />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
        },
        {
            path: '/client/orders/:status/:id',
            element: <OrderDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categories = await GetCategories();
                    const orderRes = await GetOrder(id);

                    return { id, loadedCategories: categories.data, loadedOrder: orderRes.data };
                } catch (e) {
                    console.error(e);
                    return { loadedCategories: [], loadedOrder: {} };
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