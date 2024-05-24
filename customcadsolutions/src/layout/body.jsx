import { Routes, Route } from 'react-router-dom'
import HomePage from '../public/home'
import GalleryPage from '../public/gallery'
import ChangeRolePage from '../public/changerole'
import LoginPage from '../public/login'
import RegisterPage from '../public/register'
import PrivacyPolicyPage from '../public/policy'
import OrdersPage from '../private/orders'
import CustomOrderPage from '../private/custom-order'
import GalleryOrderPage from '../private/gallery-order'
import CadsPage from '../private/cads'
import UploadCadPage from '../private/upload-cad'
import SellCadPage from '../private/sell-cad'

function Body() {
    return (
        <main className="mx-16 pb-20">
            <Routes>
                <Route path="*" element={<p className="text-center">404 not found bro</p>} />

                { /* Public part of CustomCADSolutions */}
                <Route path="/" element={<HomePage />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/gallery" element={<GalleryPage />} />
                <Route path="/changerole" element={<ChangeRolePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/register/client" element={<RegisterPage role="client" />} />
                <Route path="/register/contributor" element={<RegisterPage role="contributor" />} />
                <Route path="/policy" element={<PrivacyPolicyPage />} />

                { /* Private part of CustomCADSolutions */}
                <Route path="/orders" element={<OrdersPage />} />
                <Route path="/orders/custom" element={<CustomOrderPage />} />
                <Route path="/orders/gallery" element={<GalleryOrderPage />} />
                <Route path="/cads" element={<CadsPage />} />
                <Route path="/cads/upload" element={<UploadCadPage />} />
                <Route path="/cads/sell" element={<SellCadPage />} />
            </Routes>
        </main>
    );
}

export default Body;