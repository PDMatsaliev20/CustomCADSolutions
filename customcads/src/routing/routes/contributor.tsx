import { AxiosError } from 'axios';
import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { GetCategories } from '@/requests/public/categories';
import { GetRecentCads, GetCad, GetCadsCounts } from '@/requests/private/cads';
import ContributorHomePage from '@/pages/contributor/contributor-home/contributor-home';
import UserCadsPage from '@/pages/contributor/user-cads/cads';
import CadDetailsPage from '@/pages/contributor/cad-details/cad-details';
import UploadCadPage from '@/pages/contributor/upload-cad/upload-cad';

const contributorRoutes: RouteObject = {
    element: <AuthGuard auth="private" role="Contributor" />,
    children: [
        {
            path: '/contributor',
            element: <ContributorHomePage />,
            loader: async () => {
                try {
                    const { data: cads } = await GetRecentCads();
                    const { data: loadedCounts } = await GetCadsCounts();
                    return { loadedCads: cads, loadedCounts };
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
            path: '/contributor/cads',
            element: <UserCadsPage />
        },
        {
            path: '/contributor/cads/:id',
            element: <CadDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const categoriesRes = await GetCategories();
                    const cadRes = await GetCad(Number(id));
                    return { id: Number(id), loadedCategories: categoriesRes.data, loadedCad: cadRes.data };
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
            path: '/contributor/cads/upload',
            element: <UploadCadPage />
        },
    ]
};
export default contributorRoutes;