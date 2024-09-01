import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { useAuth } from '@/contexts/auth-context';
import { Login } from '@/requests/public/identity';
import Input from '@/components/fields/input';
import Password from '@/components/fields/password';
import { getCookie } from '@/utils/cookie-manager';
import loginValidations from './login-validations';

function LoginPage() {
    const { setIsAuthenticated } = useAuth();
    const navigate = useNavigate();
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');
    const [rememberMe, setRememberMe] = useState(false);
    const { register, formState, handleSubmit, watch } = useForm({ mode: 'onTouched' });
    const { username, password } = loginValidations();

    const onSubmit = async (user) => {
        try {
            await Login({ ...user, rememberMe });
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
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-4 flex flex-col gap-y-4">
                        <Input
                            id="username"
                            label={tCommon('labels.username')}
                            rhfProps={register('username', username)}
                            placeholder={tCommon('placeholders.username')}
                            error={formState.errors.username}
                            isRequired
                        />
                        <div className="flex items-center">
                            <Password
                                id="password"
                                label={tCommon('labels.password')}
                                placeholder={tCommon('placeholders.password')}
                                rhfProps={register('password', password)}
                                hidden={!watch('password')}
                                error={formState.errors.password}
                                className="basis-full text-indigo-900 focus:outline-none"
                                isRequired
                            />
                        </div>
                    </div>
                    <div className="flex justify-between items-center">
                        <div className="flex items-center">
                            <input
                                id="rememberMe"
                                type="checkbox"
                                name="rememberMe"
                                value={rememberMe}
                                onInput={() => setRememberMe(rm => !rm)}
                            />
                            <label htmlFor="rememberMe" className="ms-1 text-indigo-50">{tPages('login.remember_me')}</label>
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