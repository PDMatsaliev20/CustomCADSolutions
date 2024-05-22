import { Routes, Route } from 'react-router-dom'
import HomePage from '../public/home'
import GalleryPage from '../public/gallery'
import BecomeContrPage from '../public/contributor'
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
        <Routes>
            { /* Public part of CustomCADSolutions */ }
            <Route path="/home" element={<HomePage />} />
            <Route path="/gallery" element={<GalleryPage />} />
            <Route path="/contributor" element={<BecomeContrPage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/policy" element={<PrivacyPolicyPage />} />

            { /* Private part of CustomCADSolutions */ }
            <Route path="/orders" element={<OrdersPage />} />
            <Route path="/orders/order" element={<CustomOrderPage />} />
            <Route path="/gallery/order" element={<GalleryOrderPage />} />
            <Route path="/cads" element={<CadsPage />} />
            <Route path="/cads/upload" element={<UploadCadPage />} />
            <Route path="/cads/sell" element={<SellCadPage />} />
        </Routes>
    );
}

export default Body;