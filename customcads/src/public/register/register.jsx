import AuthContext from '@/components/auth-context'
import useValidateRegister from './useValidateRegister'
import userValidation from '@/constants/data/user'
import useForm from '@/hooks/useForm'
import { Register } from '@/requests/public/identity'
import { useContext } from 'react'
import { Link, useParams, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

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
                {t('body.register.Register as a')} {role == 'client' ? t('common.roles.Client') : t('common.roles.Contributor')}
            </h1>
            <div className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                    <div className="mb-4">
                        <label htmlFor="username" className="block text-indigo-50">
                            {t('common.labels.Username')}*
                        </label>
                        <input
                            id="username"
                            type="text"
                            name="username"
                            value={user.username}
                            onChange={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("body.register.Your_Username123")}
                            maxLength={userValidation.username.maxLength}
                        />
                        <span className={`${touched.username && errors.username ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                            {errors.username}
                        </span>
                    </div>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-indigo-50">
                            {t('common.labels.Email')}*
                        </label>
                        <input
                            id="email"
                            type="email"
                            name="email"
                            value={user.email}
                            onChange={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("body.register.your@email.com")}
                            maxLength={userValidation.email.maxLength}
                        />
                        <span className={`${touched.email && errors.email ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                            {errors.email}
                        </span>
                    </div>
                    <div className="mb-4">
                        <label htmlFor="password" className="block text-indigo-50">
                            {t('common.labels.Password')}*
                        </label>
                        <input
                            id="password"
                            type="password"
                            name="password"
                            value={user.password}
                            onChange={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("body.register.your_sercret_password_123")}
                            maxLength={userValidation.password.maxLength}
                        />
                        <span className={`${touched.password && errors.password ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                            {errors.password}
                        </span>
                    </div>
                    <div className="mb-2">
                        <label htmlFor="confirmPassword" className="block text-indigo-50">
                            {t('common.labels.Confirm Password')}*
                        </label>
                        <input
                            id="confirmPassword"
                            type="password"
                            name="confirmPassword"
                            value={user.confirmPassword}
                            onChange={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("body.register.your_sercret_password_123")}
                            maxLength={userValidation.password.maxLength}
                        />
                        <span className={`${touched.confirmPassword && errors.confirmPassword ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                            {errors.confirmPassword}
                        </span>
                    </div>
                    <div className="basis-full py-4 flex justify-center items-center gap-3 text-indigo-50">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 font-bold py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {t('body.register.Register')}
                        </button>
                    </div>
                </form>
            </div>
            <section className="flex gap-x-4">
                <div className="text-center">
                    <p className="text-indigo-950" >{t('body.register.Already have an account')}</p>
                    <Link to="/login" className="text-center font-semibold text-indigo-700">
                        {t('body.register.Log in')}
                    </Link>
                </div>
                <div className="text-center">
                    <p className="text-indigo-950">
                        <span>{t('body.register.Want to be')}</span>
                        <span className="font-bold"> {isClient ? t('common.roles.Contributor') : t('common.roles.Client')}?</span>
                    </p>
                    <Link to={isClient ? '/register/contributor' : '/register/client'} className="text-center font-semibold text-indigo-700">
                        {t('body.register.Switch roles')}
                    </Link>
                </div>
            </section>
        </section>
    );
}

export default RegisterPage;
