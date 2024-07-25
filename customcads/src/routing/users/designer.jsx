import AuthGuard from '../auth-guard'
import AllOrders from '@/private/designer/orders/all-orders'
import CompleteOrder from '@/private/designer/orders/complete-order'
import AllCads from '@/private/designer/cads/all-cads'
import UnvalidatedCads from '@/private/designer/cads/unvalidated-cads'

export default {
    element: <AuthGuard isPrivate role={['Designer']} />,
    children: [
        {
            path: '/orders',
            element: <AllOrders />
        },
        {
            path: '/orders/comlete',
            element: <CompleteOrder />
        },
        {
            path: '/cads',
            element: <AllCads />
        },
        {
            path: '/cads/validate',
            element: <UnvalidatedCads />
        }
    ]
};