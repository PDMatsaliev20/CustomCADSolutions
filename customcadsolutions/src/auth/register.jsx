import { useParams } from 'react-router-dom'
import { useState } from 'react'

function RegisterPage({ onRegister }) {
    const { role } = useParams();

    const isClient = role.toLowerCase() == "client";
    const isContributor = role.toLowerCase() == "contributor";

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
        <section className="flex flex-col items-center gap-8 mt-8">
            <h1 className="text-4xl text-center font-bold">
                Register as a {role == 'client' ? 'Client' : 'Contributor'}!
            </h1>
            <article className="w-5/12 px-12 py-6 bg-indigo-400 rounded-lg">
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label htmlFor="text" className="block text-indigo-50">Username</label>
                        <input
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder="Your_Username123"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-indigo-50">Email</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            placeholder="your@email.com"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="password" className="block text-indigo-50">Password</label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            required
                            placeholder="my_sercret_password_123"
                        />
                    </div>
                    <div className="mb-4">
                        <label htmlFor="confirmPassword" className="block text-indigo-50">Confirm Password</label>
                        <input
                            type="password"
                            id="confirmPassword"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            className="text-indigo-900 w-full  mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                            required
                            placeholder="my_sercret_password_123"
                        />
                    </div>
                    <div className="pt-3 flex justify-center">
                        <button
                            type="submit"
                            className="bg-indigo-600 text-indigo-50 py-2 px-4 rounded hover:bg-indigo-700"
                        >
                            Register
                        </button>
                    </div>
                </form>
            </article>
        </section>
    );
}

export default RegisterPage;