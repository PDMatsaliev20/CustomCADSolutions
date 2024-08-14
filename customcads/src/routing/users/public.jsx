import AuthGuard from '@/routing/auth-guard';
import { GalleryCad } from '@/requests/public/home';
import GalleryPage from '@/pages/public/gallery/gallery';
import GalleryDetailsPage from '@/pages/public/gallery-details/gallery-details';
import PrivacyPolicyPage from '@/pages/public/policy/policy';
import AboutUsPage from '@/pages/public/about/about';
import RoleInfoPage from '@/pages/public/role-info/role-info';
import capitalize from '@/utils/capitalize';

export default {
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
                    const { data } = await GalleryCad(id);
                    return { loadedCad: data };
                } catch (e) {
                    console.error(e);
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
                return { role: capitalize(role) };
            }
        }
    ]
};