import AuthGuard from '@/routing/auth-guard'
import ClientOrders from '@/private/designer/client-orders/orders/orders'
import CompleteOrder from '@/private/designer/client-orders/complete/complete-order'
import ContributorCads from '@/private/designer/contributor-cads/cads'

export default {
    element: <AuthGuard auth="private" roles={['Designer']} />,
    children: [
        {
            path: '/designer/cads',
            element: <ContributorCads />
        },
        {
            path: '/designer/orders/:status',
            element: <ClientOrders />,
            loader: async ({ params }) => ({ status: params.status[0].toUpperCase() + params.status.slice(1) })
        },
        {
            path: '/designer/orders/complete/:id',
            element: <CompleteOrder />
        },
    ]
};