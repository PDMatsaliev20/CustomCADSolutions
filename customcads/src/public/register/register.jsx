import AuthContext from '@/components/auth-context'
import { Link, useParams } from 'react-router-dom'
import { useState, useContext } from 'react'
import validateRegister from './validateRegister'
import userValidation from '@/constants/data/user'
import useForm from '@/hooks/useForm'
import { useTranslation } from 'react-i18next'

function RegisterPage() {
    const { onRegister } = useContext(AuthContext);
    const navigate = useNavigate();
    const { t } = useTranslation();
    const { role } = useParams();
    
    const { values: user, touched, errors, handleInput, handleBlur, handleSubmit } = useForm(
        { username: '', email: '', password: '', confirmPassword: '' },
        validateRegister
    );

    const isClient = role.toLowerCase() === "client";
    const isContributor = role.toLowerCase() === "contributor";
    if (!(isClient || isContributor)) {
        return <p className="text-4xl text-center font-bold">Can't do that, sorry</p>;
    }

    const handleSubmitCallback = () => {
        onRegister(user, isClient ? 'Client' : 'Contributor');
        navigate("/");
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="text-4xl text-center font-bold">
                {t('body.register.Register as a')} {role == 'client' ? t('common.roles.Client') : t('common.roles.Contributor')}
            </h1>
            <div className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md">
                <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                    <div className="mb-4">
                        <label htmlFor="username" className="block text-indigo-50">{t('body.register.Username')}*</label>
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
                        <label htmlFor="email" className="block text-indigo-50">{t('body.register.Email')}*</label>
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
                        <label htmlFor="password" className="block text-indigo-50">{t('body.register.Password')}*</label>
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
                        <label htmlFor="confirmPassword" className="block text-indigo-50">{t('body.register.Confirm Password')}*</label>
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
                        <button className="text-sm bg-indigo-200 text-black py-1 px-2 rounded">
                            <Link to={isClient ? '/register/contributor' : '/register/client'}>
                                {t('body.register.or switch roles')}
                            </Link>
                        </button>
                    </div>
                </form>
            </div>
            <section className="">
                <button className="">
                    <p>{t('body.register.Already have an account')}</p>
                    <Link to="/login" className="text-center font-semibold text-indigo-700">
                        {t('body.register.Log in')}
                    </Link>
                </button>
            </section>
        </section>
    );
}

export default RegisterPage;
