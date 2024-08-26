import { Link, useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '@/contexts/auth-context';
import useForm from '@/hooks/useForm';
import { Register } from '@/requests/public/identity';
import ErrorPage from '@/components/error-page';
import Input from '@/components/fields/input';
import Password from '@/components/fields/password';
import { getCookie } from '@/utils/cookie-manager';
import useValidateRegister from './register.validate';

function RegisterPage() {
    const { setIsAuthenticated } = useAuth();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const { role } = useParams();
    
    const {
        values: user,
        touched,
        errors,
        handleInput,
        handleBlur,
        handleSubmit
    } = useForm({ username: '', firstName: '', lastName: '', email: '', password: '', confirmPassword: '' }, useValidateRegister);

    const isClient = role.toLowerCase() === "client";
    const isContributor = role.toLowerCase() === "contributor";
    if (!(isClient || isContributor)) {
        return <ErrorPage status={404} />;
    }

    const handleSubmitCallback = async () => {
        try {
            await Register(isClient ? 'Client' : 'Contributor', user);
            setIsAuthenticated(true);
            navigate(`/${getCookie('role').toLowerCase()}`);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {t('public.register.register_title')} {role.toLowerCase() == 'client' ? t('common.roles.Client') : t('common.roles.Contributor')}
            </h1>
            <div className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md border-2 border-indigo-600 shadow-md shadow-indigo-500">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                    <div className="mb-2 flex flex-col gap-y-4">
                        <div className="w-full flex gap-x-2">
                            <Input
                                id="firstName"
                                label={t('common.labels.first_name')}
                                name="firstName"
                                value={user.firstName}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                placeholder={t("common.placeholders.first_name")}
                                touched={touched.firstName}
                                error={errors.firstName}
                                className="basis-1/3 grow text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            />
                            <Input
                                id="lastName"
                                label={t('common.labels.last_name')}
                                name="lastName"
                                value={user.lastName}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                placeholder={t("common.placeholders.last_name")}
                                touched={touched.lastName}
                                error={errors.lastName}
                                className="basis-1/3 grow text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            />
                        </div>
                        <Input
                            id="username"
                            label={t('common.labels.username')}
                            name="username"
                            value={user.username}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.username")}
                            touched={touched.username}
                            error={errors.username}
                            isRequired
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        />
                        <Input
                            id="email"
                            label={t('common.labels.email')}
                            type="email"
                            name="email"
                            value={user.email}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.email")}
                            touched={touched.email}
                            error={errors.email}
                            isRequired
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        />
                        <Password
                            id="password"
                            label={t('common.labels.password')}
                            name="password"
                            value={user.password}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.password")}
                            touched={touched.password}
                            error={errors.password}
                            isRequired
                            className="basis-full text-indigo-900 focus:outline-none"
                        />
                        <Password
                            id="confirmPassword"
                            label={t('common.labels.confirm_password')}
                            name="confirmPassword"
                            value={user.confirmPassword}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.password")}
                            touched={touched.confirmPassword}
                            error={errors.confirmPassword}
                            isRequired
                            className="basis-full text-indigo-900 focus:outline-none"
                        />
                    </div>
                    <div className="basis-full py-4 flex justify-center items-center gap-3 text-indigo-50">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 font-bold py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {t('public.register.register')}
                        </button>
                    </div>
                </form>
            </div>
            <section className="flex gap-x-4">
                <div className="text-center">
                    <p className="text-indigo-950" >{t('public.register.go_to_login')}</p>
                    <Link to="/login" className="text-center font-semibold text-indigo-700">
                        {t('public.register.login')}
                    </Link>
                </div>
            </section>
        </section>
    );
}

export default RegisterPage;
