import AuthGuard from '@/routing/auth-guard'
import UserCadsPage from '@/private/cads/user-cads/user-cads'
import CadDetailsPage from '@/private/cads/cad-details/cad-details'
import EditCadPage from '@/private/cads/edit-cad/edit-cad'
import UploadCadPage from '@/private/cads/upload-cad/upload-cad'
import SellCadPage from '@/private/cads/sell-cad/sell-cad'
import { GetCad } from '@/requests/private/cads'
import { GetCategories } from '@/requests/public/home'

export default {
    element: <AuthGuard auth="private" roles={['Contributor', 'Designer']} />,
    children: [
        {
            path: '/cads',
            element: <UserCadsPage />
        },
        {
            path: '/cads/:id',
            element: <CadDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const cad = await GetCad(id);
                    return { loadedCad: cad.data };
                } catch (e) {
                    console.error(e);
                }
            }
        },
        {
            path: '/cads/edit/:id',
            element: <EditCadPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categoriesRes = await GetCategories();
                    const cadRes = await GetCad(id);

                    return { loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
                } catch (e) {
                    console.error(e);
                }
            }
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