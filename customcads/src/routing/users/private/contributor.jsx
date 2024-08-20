import { Navigate } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/home';
import { GetRecentCads, GetCad, GetCadsCounts } from '@/requests/private/cads';
import ContributorHomePage from '@/pages/contributor/contributor-home';
import UserCadsPage from '@/pages/contributor/user-cads/cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';

export default {
    path: '/contributor',
    element: <AuthGuard auth="private" role="Contributor" />,
    children: [
        {
            path: '',
            element: <Navigate to="home" />
        },
        {
            path: 'home',
            element: <ContributorHomePage />,
            loader: async () => {
                try {
                    const { data: { cads } } = await GetRecentCads();
                    const { data: loadedCounts } = await GetCadsCounts();

                    return { loadedCads: cads, loadedCounts };
                } catch (e) {
                    console.error(e);
                    return { loadedCads: [], loadedCounts: { } };
                }
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
                    return {
                        id, loadedCategories: [], loadedCad: {} };
                }
            }
        },
        {
            path: 'cads/upload',
            element: <UploadCadPage />
        },
    ]
};