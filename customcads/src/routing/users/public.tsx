import { AxiosError } from 'axios';
import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { GalleryCad } from '@/requests/public/home';
import capitalize from '@/utils/capitalize';
import GalleryPage from '@/pages/public/gallery/gallery';
import GalleryDetailsPage from '@/pages/public/gallery-details/gallery-details';
import PrivacyPolicyPage from '@/pages/public/policy/policy';
import AboutUsPage from '@/pages/public/about/about';
import RoleInfoPage from '@/pages/public/role-info/role-info';

const publicRoutes: RouteObject = {
    element: <AuthGuard auth="public" />,
    children: [
        {
            path: '/gallery',
            element: <GalleryPage />
        },
        {
            path: '/gallery/:id',
            element: <GalleryDetailsPage />,
            loader: async ({ params }) => {
                const { id } = params;
                try {
                    const { data } = await GalleryCad(Number(id));
                    return { loadedCad: data };
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
            path: '/about',
            element: <AboutUsPage />
        },
        {
            path: '/policy',
            element: <PrivacyPolicyPage />
        },
        {
            path: '/info/:role',
            element: <RoleInfoPage />,
            loader: async ({ params }) => {
                const { role } = params;
                return { role: capitalize(role ?? '') };
            }
        }
    ]
};

export default publicRoutes;