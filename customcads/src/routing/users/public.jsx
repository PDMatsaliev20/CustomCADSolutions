import AuthGuard from '@/routing/auth-guard'
import HomePage from '@/public/home/home'
import GalleryPage from '@/public/gallery/gallery'
import GalleryDetailsPage from '@/public/gallery/gallery-details'
import PrivacyPolicyPage from '@/public/policy/policy'
import AboutUsPage from '@/public/about/about'
import ClientInfoPage from '@/public/info/client'
import ContributorInfoPage from '@/public/info/contributor'
import DesignerInfoPage from '@/public/info/designer'
import { GalleryCad } from '@/requests/public/home'

export default {
    element: <AuthGuard auth="public" />,
    children: [
        {
            path: '/',
            element: <HomePage />
        },
        {
            path: '/home',
            element: <HomePage />
        },
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
            path: '/info/client',
            element: <ClientInfoPage />
        },
        {
            path: '/info/contributor',
            element: <ContributorInfoPage />
        },
        {
            path: '/info/designer',
            element: <DesignerInfoPage />
        },
        {
            path: '/policy',
            element: <PrivacyPolicyPage />
        },
    ]
};