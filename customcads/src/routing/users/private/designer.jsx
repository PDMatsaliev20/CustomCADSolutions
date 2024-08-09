import AuthGuard from '@/routing/auth-guard';
import OngoingOrders from '@/private/designer/ongoing-orders/ongoing-orders';
import CompleteOrder from '@/private/designer/complete-order/complete-order';
import UncheckedCads from '@/private/designer/unchecked-cads/unchecked-cads';

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
            loader: async ({ params }) => ({ status: params.status[0].toUpperCase() + params.status.slice(1) })
        },
        {
            path: '/designer/orders/complete/:id',
            element: <CompleteOrder />
        },
    ]
};