import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetCad } from '@/requests/private/cads';
import ContributorHomePage from '@/pages/contributor/contributor-home/contributor-home';
import UserCadsPage from '@/pages/contributor/user-cads/user-cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';

export default {
    path: '/contributor',
    element: <AuthGuard auth="private" role="Contributor" />,
    children: [
        {
            element: <ContributorHomePage />
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

                    return { loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
                } catch (e) {
                    console.error(e);
                }
            }
        },
        {
            path: 'cads/upload',
            element: <UploadCadPage />
        },
    ]
};