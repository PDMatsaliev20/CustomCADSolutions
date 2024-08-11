import { useContext } from 'react';
import { Link, useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import AuthContext from '@/contexts/auth-context';
import { Register } from '@/requests/public/identity';
import Input from '@/components/input';
import useValidateRegister from './register.validate';

function RegisterPage() {
    const { setIsAuthenticated } = useContext(AuthContext);
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
    } = useForm({ username: '', email: '', password: '', confirmPassword: '' }, useValidateRegister);

    const isClient = role.toLowerCase() === "client";
    const isContributor = role.toLowerCase() === "contributor";
    if (!(isClient || isContributor)) {
        return <p className="text-4xl text-center font-bold">Can't do that, sorry</p>;
    }

    const handleSubmitCallback = async () => {
        try {
            await Register(user, isClient ? 'Client' : 'Contributor');
            setIsAuthenticated(true);
            navigate("/");
        } catch (e) {
            alert(e.response.data);
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {t('public.register.register_title')} {role.toLowerCase() == 'client' ? t('common.roles.Client') : t('common.roles.Contributor')}
            </h1>
            <div className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                    <div className="mb-2 flex flex-col gap-y-4">
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
                        />
                        <Input
                            id="password"
                            label={t('common.labels.password')}
                            type="password"
                            name="password"
                            value={user.password}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.password")}
                            touched={touched.password}
                            error={errors.password}
                            isRequired
                        />
                         <Input
                            id="confirmPassword"
                            label={t('common.labels.confirm_password')}
                            type="password"
                            name="confirmPassword"
                            value={user.confirmPassword}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t("common.placeholders.password")}
                            touched={touched.confirmPassword}
                            error={errors.confirmPassword}
                            isRequired
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
                <div className="text-center">
                    <p className="text-indigo-950">
                        <span>{t('public.register.want')}</span>
                        <span className="font-bold"> {isClient ? t('common.roles.Contributor') : t('common.roles.Client')}?</span>
                    </p>
                    <Link to={isClient ? '/register/contributor' : '/register/client'} className="text-center font-semibold text-indigo-700">
                        {t('public.register.switch')}
                    </Link>
                </div>
            </section>
        </section>
    );
}

export default RegisterPage;
