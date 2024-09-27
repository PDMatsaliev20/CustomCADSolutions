import { AxiosError } from 'axios';
import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/categories';
import { GetCad } from '@/requests/private/cads';
import { GetUncheckedCad, GetOngoingOrder } from '@/requests/private/designer';
import DesignerHomePage from '@/pages/designer/designer-home';
import UserCadsPage from '@/pages/contributor/user-cads/cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';
import OngoingOrders from '@/pages/designer/ongoing-orders/ongoing-orders';
import OngoingOrderDetails from '@/pages/designer/ongoing-order-details/ongoing-order-details';
import UncheckedCads from '@/pages/designer/unchecked-cads/unchecked-cads';
import UncheckedCadDetails from '@/pages/designer/unchecked-cad-details/unchecked-cad-details';

const designerRoutes: RouteObject = {
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
                    const cadRes = await GetCad(Number(id));

                    return { id, loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
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
                    const res = await GetUncheckedCad(Number(id));
                    return { prevId: res.data.prevId, loadedCad: res.data.cad, nextId: res.data.nextId };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
                }
            }
        },
        {
            path: '/designer/orders/:status',
            element: <OngoingOrders />,
        },
        {
            path: '/designer/orders/:status/:id',
            element: <OngoingOrderDetails />,
            loader: async ({ params }) => {
                try {
                    const { id } = params;
                    const res = await GetOngoingOrder(Number(id));
                    return { loadedOrder: res.data };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
                }
            }
        },
    ]
};
export default designerRoutes;