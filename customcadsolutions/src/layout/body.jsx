import { Routes, Route } from 'react-router-dom'
import HomePage from '../public/home'
import GalleryPage from '../public/gallery'
import LoginPage from '../auth/login'
import RegisterPage from '../auth/register'
import ChooseRolePage from '../auth/choose-role'
import PrivacyPolicyPage from '../public/policy'
import OrdersPage from '../private/orders'
import CustomOrderPage from '../private/custom-order'
import GalleryOrderPage from '../private/gallery-order'
import CadsPage from '../private/cads'
import UploadCadPage from '../private/upload-cad'
import SellCadPage from '../private/sell-cad'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'

function Body({ setIsAuthenticated }) {
    const navigate = useNavigate();

    const register = async (user, userRole) => {
        const { token, role, username } = await axios.post(`https://localhost:7127/API/Account/Register/${userRole}`, user)
            .then(response => response.data);

        localStorage.setItem('token', token);
        localStorage.setItem('username', username);
        localStorage.setItem('role', role);

        setIsAuthenticated(true);

        navigate("/");
    };

    const login = async (user) => {
        const { token, role, username } = await axios.post(`https://localhost:7127/API/Account/Login`, user)
            .then(response => response.data);

        localStorage.setItem('token', token);
        localStorage.setItem('username', username);
        localStorage.setItem('role', role);

        setIsAuthenticated(true);

        navigate("/");
    };

    const isInRole = (role) => localStorage.getItem('role') === role;

    return (
        <main className="mx-16 pb-20">
            <Routes>
                <Route path="*" element={<p className="text-center">404 not found bro</p>} />

                { /* Public part of CustomCADSolutions */}
                <Route path="/" element={<HomePage />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/gallery" element={<GalleryPage />} />
                <Route path="/login" element={<LoginPage onLogin={login} />} />
                <Route path="/register" element={<ChooseRolePage />} />
                <Route path="/register/:role" element={<RegisterPage onRegister={register} />} />
                <Route path="/policy" element={<PrivacyPolicyPage />} />

                { /* Private part of CustomCADSolutions */}
                <Route path="/orders" element={isInRole('Contributor') ?
                    <OrdersPage /> : <p className="text-center">403 unauthorized bro</p>} />

                <Route path="/orders/custom" element={isInRole('Contributor') ?
                    <CustomOrderPage /> : <p className="text-center">403 unauthorized bro</p>} />
                <Route path="/orders/gallery" element={isInRole('Contributor') ?
                    <GalleryOrderPage /> : <p className="text-center">403 unauthorized bro</p>} />
                <Route path="/cads" element={isInRole('Contributor') ?
                    <CadsPage /> : <p className="text-center">403 unauthorized bro</p>} />
                <Route path="/cads/upload" element={isInRole('Contributor') ?
                    <UploadCadPage /> : <p className="text-center">403 unauthorized bro</p>} />
                <Route path="/cads/sell" element={isInRole('Contributor') ?
                    <SellCadPage /> : <p className="text-center">403 unauthorized bro</p>} />
            </Routes>
        </main>
    );
}

export default Body;