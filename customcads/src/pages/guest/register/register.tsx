import { Link, useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { Register } from '@/requests/public/identity';
import ErrorPage from '@/components/error-page';
import Input from '@/components/fields/input';
import Password from '@/components/fields/password';
import capitalize from '@/utils/capitalize';
import registerValidations from './register-validations';
import IRegister from '@/interfaces/register';

function RegisterPage() {
    const navigate = useNavigate();
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');
    const { role } = useParams();
    const { register, formState, handleSubmit, watch } = useForm<IRegister>({ mode: 'onTouched' });
    const { firstName, lastName, username, email, password, confirmPassword } = registerValidations(watch('password'));

    const isClient = role!.toLowerCase() === "client";
    const isContributor = role!.toLowerCase() === "contributor";
    if (!(isClient || isContributor)) {
        return <ErrorPage status={404} />;
    }

    const onSubmit = async (user: IRegister) => {
        try {
            await Register(isClient ? 'Client' : 'Contributor', user);
            navigate(`/register/verify-email/${user.username}`);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {tPages('register.register_title', { role: tCommon(`roles.${capitalize(role!)}`) })}
            </h1>
            <div className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md border-2 border-indigo-600 shadow-md shadow-indigo-500">
                <form onSubmit={handleSubmit(onSubmit)} noValidate>
                    <div className="mb-2 flex flex-col gap-y-4">
                        <div className="w-full flex gap-x-2">
                            <Input
                                id="firstName"
                                label={tCommon('labels.first_name')}
                                rhfProps={register('firstName', firstName)}
                                placeholder={tCommon("placeholders.first_name")}
                                error={formState.errors.firstName}
                                className="basis-1/3 grow text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            />
                            <Input
                                id="lastName"
                                label={tCommon('labels.last_name')}
                                rhfProps={register('lastName', lastName)}
                                placeholder={tCommon("placeholders.last_name")}
                                error={formState.errors.lastName}
                                className="basis-1/3 grow text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            />
                        </div>
                        <Input
                            id="username"
                            label={tCommon('labels.username')}
                            rhfProps={register('username', username)}
                            placeholder={tCommon('placeholders.username')}
                            error={formState.errors.username}
                            isRequired
                        />
                        <Input
                            id="email"
                            label={tCommon('labels.email')}
                            type="email"
                            rhfProps={register('email', email)}
                            placeholder={tCommon("placeholders.email")}
                            error={formState.errors.email}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border-2 border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            isRequired
                        />
                        <Password
                            id="password"
                            label={tCommon('labels.password')}
                            name="password"
                            placeholder={tCommon('placeholders.password')}
                            rhfProps={register('password', password)}
                            hidden={!watch('password')}
                            error={formState.errors.password}
                            className="basis-full text-indigo-900 focus:outline-none"
                            isRequired
                        />
                        <Password
                            id="confirmPassword"
                            label={tCommon('labels.confirm_password')}
                            rhfProps={register('confirmPassword', confirmPassword)}
                            placeholder={tCommon("placeholders.password")}
                            hidden={!watch('confirmPassword')}
                            error={formState.errors.confirmPassword}
                            className="basis-full text-indigo-900 focus:outline-none"
                            isRequired
                        />
                    </div>
                    <div className="basis-full py-4 flex justify-center items-center gap-3 text-indigo-50">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 font-bold py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {tPages('register.register')}
                        </button>
                    </div>
                </form>
            </div>
            <section className="flex gap-x-4">
                <div className="text-center">
                    <p className="text-indigo-950" >{tPages('register.go_to_login')}</p>
                    <Link to="/login" className="text-center font-semibold text-indigo-700">
                        {tPages('register.login')}
                    </Link>
                </div>
            </section>
        </section>
    );
}

export default RegisterPage;
