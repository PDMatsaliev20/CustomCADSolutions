import AuthGuard from '@/components/auth-guard'
import UserOrdersPage from '@/private/orders/user-orders'
import OrderDetailsPage from '@/private/orders/CRUD/order-details/order-details'
import CustomOrderPage from '@/private/orders/CRUD/custom-order/custom-order'
import GalleryOrderPage from '@/private/orders/CRUD/gallery-order/gallery-order'
import UserCadsPage from '@/private/cads/user-cads'
import UploadCadPage from '@/private/cads/upload-cad/upload-cad'
import SellCadPage from '@/private/cads/sell-cad/sell-cad'
import axios from 'axios'

export default {
    element: <AuthGuard isPrivate />,
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
            path: '/orders/:id',
            element: <OrderDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                const categories = await axios.get('https://localhost:7127/API/Categories', {
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
        {
            path: '/orders/gallery',
            element: <GalleryOrderPage />
        },
        {
            path: '/orders/gallery',
            element: <GalleryOrderPage />
        },
        {
            path: '/cads',
            element: <UserCadsPage />
        },
        {
            path: '/cads/upload',
            element: <UploadCadPage />
        },
        {
            path: '/cads/sell',
            element: <SellCadPage />
        },
    ]
};