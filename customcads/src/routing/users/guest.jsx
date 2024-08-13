import AuthGuard from '@/routing/auth-guard';
import LoginPage from '@/pages/guest/login/login';
import RegisterPage from '@/pages/guest/register/register';
import ChooseRolePage from '@/pages/guest/register/choose-role';

export default {
    element: <AuthGuard auth="guest" />,
    children: [
        {
            path: "/login",
            element: <LoginPage />
        },
        {
            path: "/register",
            element: <ChooseRolePage />
        },
        {
            path: "/register/:role",
            element: <RegisterPage />
        }
    ]
};
