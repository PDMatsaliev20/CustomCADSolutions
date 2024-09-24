import { AxiosError } from 'axios';
import { RouteObject } from 'react-router-dom';
import AuthGuard from '@/routing/auth-guard';
import { IsEmailConfirmed, DoesUserExist } from '@/requests/public/identity';
import HomePage from '@/pages/guest/home/home';
import LoginPage from '@/pages/guest/login/login';
import ForgotPasswordPage from '@/pages/guest/login/forgot-password';
import ResetPasswordPage from '@/pages/guest/login/reset-password';
import RegisterPage from '@/pages/guest/register/register';
import VerifyEmailPage from '@/pages/guest/register/verify-email';
import ChooseRolePage from '@/pages/guest/register/choose-role';


const guestRoutes : RouteObject = {
    element: <AuthGuard auth="guest" />,
    children: [
        {
            path: "/",
            element: <HomePage />
        },
        {
            path: "/login",
            element: <LoginPage />
        },
        {
            path: "/login/forgot-password",
            element: <ForgotPasswordPage />
        },
        {
            path: "/login/reset-password",
            element: <ResetPasswordPage />
        },
        {
            path: "/register",
            element: <ChooseRolePage />
        },
        {
            path: "/register/verify-email/:username",
            element: <VerifyEmailPage />,
            loader: async ({ params }) => {
                const { username } = params;
                try {
                    const { data: isEmailConfirmed } = await IsEmailConfirmed(username ?? '');
                    const { data: doesUserExist } = await DoesUserExist(username ?? '');
                    return { username, isEmailConfirmed, doesUserExist };
                } catch (e) {
                    const res = { error: true };
                    if (!(e instanceof AxiosError)) {
                        return res;
                    }
                    return { ...res, status: e.response!.status };;
                }
            }
        },
        {
            path: "/register/:role",
            element: <RegisterPage />
        }
    ]
};
export default guestRoutes;
