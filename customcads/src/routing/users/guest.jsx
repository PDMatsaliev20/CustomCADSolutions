import AuthGuard from '@/routing/auth-guard';
import LoginPage from '@/public/login/login';
import RegisterPage from '@/public/register/register';
import ChooseRolePage from '@/public/register/choose-role';

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
            element: < RegisterPage />
        }
    ]
};
