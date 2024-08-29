import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '@/contexts/auth-context';
import useForm from '@/hooks/useForm';
import { Login } from '@/requests/public/identity';
import Input from '@/components/fields/input';
import Password from '@/components/fields/password';
import { getCookie } from '@/utils/cookie-manager';
import useValidateLogin from './login.validate';

function LoginPage() {
    const { setIsAuthenticated } = useAuth();
    const navigate = useNavigate();
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');

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

            const role = getCookie('role');
            navigate(`/${role.toLowerCase()}`);
        } catch (e) {
            const { status, data } = e.response;
            switch (status) {
                case 401: alert(tCommon('errors.sign_in_error')); break;
                case 423: alert(tCommon('errors.locked_out_error', { seconds: data.seconds })); break;
                default: break;
            }
            console.error(e);
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="my-6 text-4xl text-center font-bold ">
                {tPages('login.title')}
            </h1>
            <div className="w-6/12 px-12 pt-8 pb-6 bg-indigo-400 rounded-lg border-2 border-indigo-600 shadow-md shadow-indigo-500">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} noValidate>
                    <div className="mb-4 flex flex-col gap-y-4">
                        <Input
                            id="username"
                            label={tCommon('labels.username')}
                            name="username"
                            value={user.username}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={tCommon('placeholders.username')}
                            touched={touched.username} 
                            error={errors.username} 
                            isRequired
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        />
                        <div className="flex items-center">
                            <Password
                                id="password"
                                label={tCommon('labels.password')}
                                name="password"
                                value={user.password}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                placeholder={tCommon('placeholders.password')}
                                touched={touched.password}
                                error={errors.password}
                                isRequired
                                className="basis-full text-indigo-900 focus:outline-none"
                            />
                        </div>
                    </div>
                    <div className="flex justify-between items-center">
                        <div className="flex items-center">
                            <input
                                type="checkbox"
                                name="rememberMe"
                                value={user.rememberMe}
                                onInput={handleInput}
                            />
                            <label className="ms-1 text-indigo-50">{tPages('login.remember_me')}</label>
                        </div>
                        <div>
                            <Link to="#" className="text-indigo-900 font-bold underline-offset-2 underline hover:italic">
                                {tPages('login.forgot_password')}
                            </Link>
                        </div>
                    </div>
                    <div className="pt-2 flex justify-center">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {tPages('login.log_in')}
                        </button>
                    </div>
                </form>
            </div>
            <section className="">
                <button className="">
                    <p>{tPages("login.go_to_register")}</p>
                    <Link to="/register" className="text-center font-bold text-indigo-700">
                        {tPages('login.register')}
                    </Link>
                </button>
            </section>
        </section>
    );
}

export default LoginPage;