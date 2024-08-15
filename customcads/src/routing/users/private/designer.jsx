import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetCad, GetRecentCads } from '@/requests/private/cads';
import { GetRecentFinishedOrders } from '@/requests/private/designer';
import DesignerHomePage from '@/pages/designer/designer-home';
import UserCadsPage from '@/pages/contributor/user-cads/cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';
import OngoingOrders from '@/pages/designer/ongoing-orders/ongoing-orders';
import UncheckedCads from '@/pages/designer/unchecked-cads/unchecked-cads';
import capitalize from '@/utils/capitalize';

export default {
    path: '/designer',
    element: <AuthGuard auth="private" role="Designer" />,
    children: [
        {
            path: '',
            element: <DesignerHomePage />,
            loader: async () => {
                const { data: { cads } } = await GetRecentCads();
                const { data: { orders } } = await GetRecentFinishedOrders();

                return { loadedCads: cads, loadedOrders: orders };
            }
        },
        {
            path: 'cads',
            element: <UserCadsPage />
        },
        {
            path: 'cads/:id',
            element: <CadDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categoriesRes = await GetCategories();
                    const cadRes = await GetCad(id);
                    
                    return { id, loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
                } catch (e) {
                    console.error(e);
                }
            }
        },
        {
            path: 'cads/upload',
            element: <UploadCadPage />
        },
        {
            path: 'cads/upload/:id',
            element: <UploadCadPage />
        },
        {
            path: 'cads/unchecked',
            element: <UncheckedCads />
        },
        {
            path: 'orders/:status',
            element: <OngoingOrders />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
        },
    ]
};