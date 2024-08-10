import { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import AuthContext from '@/contexts/auth-context';
import { Login } from '@/requests/public/identity';
import Input from '@/components/input';
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
                    <div className="mb-4 flex flex-col gap-y-4">
                        <Input
                            id="username"
                            label={t('common.labels.Username')}
                            name="username"
                            value={user.username}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t('body.login.Your_Username123')}
                            touched={touched.username} 
                            error={errors.username} 
                            isRequired
                        />
                        <Input
                            id="password"
                            label={t('common.labels.Password')}
                            type="password"
                            name="password"
                            value={user.password}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t('body.login.your_secret_password_123')}
                            touched={touched.password} 
                            error={errors.password} 
                            isRequired
                        />
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