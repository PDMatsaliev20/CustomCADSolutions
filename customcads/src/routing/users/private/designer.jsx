import AuthGuard from '@/routing/auth-guard';
import OngoingOrders from '@/pages/private/designer/ongoing-orders/ongoing-orders';
import CompleteOrder from '@/pages/private/designer/complete-order/complete-order';
import UncheckedCads from '@/pages/private/designer/unchecked-cads/unchecked-cads';
import capitalize from '@/utils/capitalize';

export default {
    element: <AuthGuard auth="private" roles={['Designer']} />,
    children: [
        {
            path: '/designer/cads',
            element: <UncheckedCads />
        },
        {
            path: '/designer/orders/:status',
            element: <OngoingOrders />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
        },
        {
            path: '/designer/orders/complete/:id',
            element: <CompleteOrder />
        },
    ]
};