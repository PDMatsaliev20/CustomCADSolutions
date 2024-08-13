import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetOrder } from '@/requests/private/orders';
import UserOrdersPage from '@/pages/private/orders/user-orders/orders';
import OrderDetailsPage from '@/pages/private/orders/order-details/order-details';
import PurchasePage from '@/pages/private/orders/purchase/purchase';
import CustomOrderPage from '@/pages/private/orders/custom-order/custom-order';
import capitalize from '@/utils/capitalize';

export default {
    element: <AuthGuard auth="private" roles={['Client']} />,
    children: [
        {
            path: '/orders/:status',
            element: <UserOrdersPage />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
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
        {
            path: '/purchase/:id',
            element: <PurchasePage />
        },
    ]
};