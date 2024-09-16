import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetCad } from '@/requests/private/cads';
import { GetUncheckedCad } from '@/requests/private/designer';
import DesignerHomePage from '@/pages/designer/designer-home';
import UserCadsPage from '@/pages/contributor/user-cads/cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';
import OngoingOrders from '@/pages/designer/ongoing-orders/ongoing-orders';
import UncheckedCads from '@/pages/designer/unchecked-cads/unchecked-cads';
import UncheckedCadDetails from '@/pages/designer/unchecked-cad-details/unchecked-cad-details';
import capitalize from '@/utils/capitalize';

export default {
    element: <AuthGuard auth="private" role="Designer" />,
    children: [
        {
            path: '/designer',
            element: <DesignerHomePage />
        },
        {
            path: '/designer/cads',
            element: <UserCadsPage />
        },
        {
            path: '/designer/cads/:id',
            element: <CadDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categoriesRes = await GetCategories();
                    const cadRes = await GetCad(id);

                    return { id, loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
                } catch (e) {
                    console.error(e);
                    return { id, loadedCategories: [], loadedCad: [] };
                }
            }
        },
        {
            path: '/designer/cads/upload',
            element: <UploadCadPage />
        },
        {
            path: '/designer/cads/upload/:id',
            element: <UploadCadPage />
        },
        {
            path: '/designer/cads/unchecked',
            element: <UncheckedCads />
        },
        {
            path: '/designer/cads/unchecked/:id',
            element: <UncheckedCadDetails />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const res = await GetUncheckedCad(id);

                    return { prevId: res.data.prevId, loadedCad: res.data.cad, nextId: res.data.nextId };
                } catch (e) {
                    console.error(e);
                    const response = {
                        error: true
                    };
                    switch (e.response.status) {
                        case 401: return { ...response, unauthenticated: true };
                        case 403: return { ...response, unauthorized: true };
                        default: return { ...response, status: e.response.status };
                    }
                }
            }
        },
        {
            path: '/designer/orders/:status',
            element: <OngoingOrders />,
            loader: async ({ params }) => ({ status: capitalize(params.status) })
        },
    ]
};