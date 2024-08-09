import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetCad } from '@/requests/private/cads';
import UserCadsPage from '@/private/cads/user-cads/user-cads';
import CadDetailsPage from '@/private/cads/cad-details/cad-details';
import UploadCadPage from '@/private/cads/upload-cad/upload-cad';

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
    ]
};