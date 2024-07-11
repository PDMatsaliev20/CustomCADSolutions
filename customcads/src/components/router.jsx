import { Routes, Route } from 'react-router-dom'
import HomePage from '@/public/home/home'
import GalleryPage from '@/public/gallery/gallery'
import LoginPage from '@/auth/login'
import RegisterPage from '@/auth/register'
import ChooseRolePage from '@/auth/choose-role'
import PrivacyPolicyPage from '@/public/policy/policy'
import AboutUsPage from '@/public/about/about'
import OrdersPage from '@/private/orders'
import CustomOrderPage from '@/private/custom-order'
import GalleryOrderPage from '@/private/gallery-order'
import CadsPage from '@/private/cads'
import UploadCadPage from '@/private/upload-cad'
import SellCadPage from '@/private/sell-cad'
import AuthGuard from '@/auth/authguard'

function Router({ onLogin, onRegister }) {
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
                <Route path="/orders" element={<OrdersPage /> } />
                <Route path="/orders/custom" element={<CustomOrderPage /> } />
                <Route path="/orders/gallery" element={<GalleryOrderPage /> } />
                <Route path="/cads" element={<CadsPage /> } />
                <Route path="/cads/upload" element={<UploadCadPage /> } />
                <Route path="/cads/sell" element={<SellCadPage /> } />
            </Route>

            <Route element={<AuthGuard isPrivateOnly />}>
                <Route path="/login" element={<LoginPage onLogin={onLogin} />} />
                <Route path="/register" element={<ChooseRolePage />} />
                <Route path="/register/:role" element={<RegisterPage onRegister={onRegister} />} />
            </Route>
        </Routes>
    );
}

export default Router;