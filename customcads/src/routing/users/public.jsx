import AuthGuard from '@/components/auth-guard'
import HomePage from '@/public/home/home'
import GalleryPage from '@/public/gallery/gallery'
import PrivacyPolicyPage from '@/public/policy/policy'
import AboutUsPage from '@/public/about/about'

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
            path: '/about',
            element: <AboutUsPage />
        },
        {
            path: '/policy',
            element: <PrivacyPolicyPage />
        },
    ]
};