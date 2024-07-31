import AuthGuard from '../auth-guard'
import HomePage from '@/public/home/home'
import GalleryPage from '@/public/gallery/gallery'
import GalleryDetailsPage from '@/public/gallery/gallery-details'
import PrivacyPolicyPage from '@/public/policy/policy'
import AboutUsPage from '@/public/about/about'
import axios from 'axios'

export default {
    element: <AuthGuard />,
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
                const cad = await axios.get(`https://localhost:7127/API/Home/Gallery/${id}`, {
                    withCredentials: true
                }).catch(e => console.error(e));

                return { loadedCad: cad.data };
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
    ]
};