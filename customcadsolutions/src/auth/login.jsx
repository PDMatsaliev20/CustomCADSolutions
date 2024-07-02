import { useState, useTransition } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function LoginPage({ onLogin }) {

    const { t } = useTranslation();

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);


    const handleSubmit = (event) => {
        event.preventDefault();
        onLogin({ username, password });
    }

    return (
        <section className="flex flex-col items-center">
            <h1 className="text-4xl text-center font-bold ">
                {t('Log in to your existing account')}
            </h1>
            <section className="w-5/12 mt-8 px-12 pt-8 pb-6 bg-indigo-400 rounded-lg">
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
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
                    <div className="mb-4">
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
                    <div className="flex justify-between items-center">
                        <div className="flex items-center">
                            <input type="checkbox" onClick={() => setRememberMe(!rememberMe)} />
                            <label className="ms-1 text-indigo-50">{t('Remember me')}</label>
                        </div>
                        <div>
                            <Link to="#" className="text-indigo-800">{t('Forgot password')}</Link>
                        </div>
                    </div>
                    <div className="pt-2 flex justify-center">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            {t('Log in')}
                        </button>
                    </div>
                </form>
            </section>
        </section>
    );
}

export default LoginPage;