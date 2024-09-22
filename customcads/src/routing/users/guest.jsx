import AuthGuard from '@/routing/auth-guard';
import HomePage from '@/pages/guest/home/home';
import LoginPage from '@/pages/guest/login/login';
import ForgotPasswordPage from '@/pages/guest/login/forgot-password';
import ResetPasswordPage from '@/pages/guest/login/reset-password';
import RegisterPage from '@/pages/guest/register/register';
import VerifyEmailPage from '@/pages/guest/register/verify-email';
import ChooseRolePage from '@/pages/guest/register/choose-role';
import { IsEmailConfirmed, DoesUserExist } from '@/requests/public/identity';

export default {
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
                    const { data: isEmailConfirmed } = await IsEmailConfirmed(username);
                    const { data: doesUserExist } = await DoesUserExist(username);

                    return { username, isEmailConfirmed, doesUserExist };
                } catch (e) {
                    console.error(e);
                    return { username, isEmailConfirmed: false, doesUserExist: false };
                }
            }
        },
        {
            path: "/register/:role",
            element: <RegisterPage />
        }
    ]
};
