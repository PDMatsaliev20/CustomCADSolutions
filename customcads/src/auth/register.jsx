import { Link, useParams } from 'react-router-dom'
import { useState } from 'react'
import { useTranslation } from 'react-i18next'

function RegisterPage({ onRegister }) {
    const { t } = useTranslation();
    const { role } = useParams();

    const isClient = role.toLowerCase() === "client";
    const isContributor = role.toLowerCase() === "contributor";

    if (!(isClient || isContributor)) {
        return <p className="text-4xl text-center font-bold">Can't do that, sorry</p>;
    }

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const handleSubmit = (event) => {
        event.preventDefault();
        if (password != confirmPassword) {
            alert('passwords do not match bro');
        }
        else {
            onRegister({ username, email, password, confirmPassword }, isClient ? 'Client' : 'Contributor');
        }
    }

    return (
        <section className="flex flex-col gap-y-4 items-center">
            <h1 className="text-4xl text-center font-bold">
                {t('Register as a')} {role == 'client' ? t('Client') : t('Contributor')}
            </h1>
            <section className="w-7/12 pt-8 pb-2 px-12 mt-8 bg-indigo-400 rounded-md">
                <form onSubmit={handleSubmit} className="flex flex-wrap gap-x-8">
                    <div className="mb-4 basis-5/12 grow">
                        <label htmlFor="text" className="block text-indigo-50">{t('Username')}</label>
                        <input
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("Your_Username123")}
                            required
                        />
                    </div>
                    <div className="mb-4 basis-5/12 grow">
                        <label htmlFor="email" className="block text-indigo-50">{t('Email')}</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder={t("your@email.com")}
                            required
                        />
                    </div>
                    <div className="mb-4 basis-5/12 grow">
                        <label htmlFor="password" className="block text-indigo-50">{t('Password')}</label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            required
                            placeholder={t("your_sercret_password_123")}
                        />
                    </div>
                    <div className="mb-2 basis-5/12 grow">
                        <label htmlFor="confirmPassword" className="block text-indigo-50">{t('Confirm Password')}</label>
                        <input
                            type="password"
                            id="confirmPassword"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            required
                            placeholder={t("your_sercret_password_123")}
                        />
                    </div>
                    <div className="basis-full py-4 flex justify-center items-center gap-3 text-indigo-50">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 font-bold py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {t('Register')}
                        </button>
                        <button className="text-sm bg-indigo-200 text-black py-1 px-2 rounded">
                            <Link to={isClient ? '/register/contributor' : '/register/client'}>
                                {t('or switch roles')}
                            </Link>
                        </button>
                    </div>
                </form>
            </section>
            <section className="">
                <button className="">
                    <p>Already have an account?</p>
                    <Link to="/login" className="text-center font-semibold text-indigo-700">
                        Log in
                    </Link>
                </button>
            </section>
        </section>
    );
}

export default RegisterPage;