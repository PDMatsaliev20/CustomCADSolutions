import { Routes, Route } from 'react-router-dom'
import AuthGuard from '@/components/auth-guard'

import HomePage from '@/public/home/home'
import GalleryPage from '@/public/gallery/gallery'
import PrivacyPolicyPage from '@/public/policy/policy'
import AboutUsPage from '@/public/about/about'

import LoginPage from '@/public/login/login'
import RegisterPage from '@/public/register/register'
import ChooseRolePage from '@/public/register/choose-role'

import UserOrdersPage from '@/private/orders/user-orders'
import OrderDetailsPage from '@/private/orders/CRUD/order-details/order-details'
import CustomOrderPage from '@/private/orders/CRUD/custom-order/custom-order'
import GalleryOrderPage from '@/private/orders/CRUD/gallery-order/gallery-order'

import UserCadsPage from '@/private/cads/user-cads'
import UploadCadPage from '@/private/cads/upload-cad/upload-cad'
import SellCadPage from '@/private/cads/sell-cad/sell-cad'

function Router() {
    return (
        <Routes>
            <Route path="*" element={<p className="text-center">404 not found bro</p>} />

            { /* Public part of CustomCADs */}
            <Route element={<AuthGuard />}>
                <Route path="/" element={<HomePage />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/gallery" element={<GalleryPage />} />
                <Route path="/policy" element={<PrivacyPolicyPage />} />
                <Route path="/about" element={<AboutUsPage />} />
            </Route>

            { /* Private part of CustomCADs */}
            <Route element={<AuthGuard isPrivate />}>
                <Route path="/orders" element={<UserOrdersPage /> } />
                <Route path="/orders/:id" element={<OrderDetailsPage /> } />
                <Route path="/orders/custom" element={<CustomOrderPage /> } />
                <Route path="/orders/gallery" element={<GalleryOrderPage /> } />
                <Route path="/cads" element={<UserCadsPage /> } />
                <Route path="/cads/upload" element={<UploadCadPage /> } />
                <Route path="/cads/sell" element={<SellCadPage /> } />
            </Route>

            {/* Public only part of CustomCADs */}
            <Route element={<AuthGuard isPublicOnly />}>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<ChooseRolePage />} />
                <Route path="/register/:role" element={<RegisterPage />} />
            </Route>
        </Routes>
    );
}

export default Router;