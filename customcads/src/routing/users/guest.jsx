import AuthGuard from '@/routing/auth-guard';
import HomePage from '@/pages/guest/home/home';
import RoleInfoPage from '@/pages/guest/role-info/role-info';
import LoginPage from '@/pages/guest/login/login';
import RegisterPage from '@/pages/guest/register/register';
import ChooseRolePage from '@/pages/guest/register/choose-role';
import capitalize from '@/utils/capitalize';

export default {
    element: <AuthGuard auth="guest" />,
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
        },
        {
            path: '/info/:role',
            element: <RoleInfoPage />,
            loader: async ({ params }) => {
                const { role } = params;
                return { role: capitalize(role) };
            }
        }
    ]
};
