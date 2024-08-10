import { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import AuthContext from '@/contexts/auth-context';
import { Login } from '@/requests/public/identity';
import useValidateLogin from './login.validate';

function LoginPage() {
    const { setIsAuthenticated } = useContext(AuthContext);
    const navigate = useNavigate();
    const { t } = useTranslation();

    const {
        values: user,
        touched,
        errors,
        handleInput,
        handleBlur,
        handleSubmit,
    } = useForm({ username: '', password: '', rememberMe: false }, useValidateLogin);

    const handleSubmitCallback = async () => {
        try {
            await Login(user);
            setIsAuthenticated(true);
            navigate("/");
        } catch (e) {
            alert(e.response.data);
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="my-6 text-4xl text-center font-bold ">
                {t('body.login.Log in to your existing account')}
            </h1>
            <div className="w-6/12 px-12 pt-8 pb-6 bg-indigo-400 rounded-lg">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} noValidate>
                    <div className="mb-4">
                        <label htmlFor="username" className="block text-indigo-50">
                            {t('common.labels.Username')}*
                        </label>
                        <input
                            id="username"
                            type="text"
                            name="username"
                            value={user.username}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("body.login.Your_Username123")}
                            required
                        />
                        <span className={`${touched.username && errors.username ? 'inline-block' : 'hidden'} mt-1 text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}
                        >
                            {errors.username}
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
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            required
                            placeholder={t("body.login.your_sercret_password_123")}
                        />
                        <span className={`${touched.password && errors.password ? 'inline-block' : 'hidden'} mt-1 text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}
                        >
                            {errors.password}
                        </span>
                    </div>
                    <div className="flex justify-between items-center">
                        <div className="flex items-center">
                            <input
                                type="checkbox"
                                name="rememberMe"
                                value={user.rememberMe}
                                onInput={handleInput}
                            />
                            <label className="ms-1 text-indigo-50">{t('body.login.Remember me')}</label>
                        </div>
                        <div>
                            <Link to="#" className="text-indigo-800">{t('body.login.Forgot password')}</Link>
                        </div>
                    </div>
                    <div className="pt-2 flex justify-center">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {t('body.login.Log in')}
                        </button>
                    </div>
                </form>
            </div>
            <section className="">
                <button className="">
                    <p>{t("body.login.Don't have an account yet")}</p>
                    <Link to="/register" className="text-center font-semibold text-indigo-700">
                        {t('body.login.Register')}
                    </Link>
                </button>
            </section>
        </section>
    );
}

export default LoginPage;