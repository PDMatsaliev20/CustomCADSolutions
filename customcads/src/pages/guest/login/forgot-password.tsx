import { useState } from 'react';
import { ForgotPassword } from '@/requests/public/identity';

function ForgotPasswordPage() {
    const [email, setEmail] = useState('');

    const handleClick = async () => {
        try {
            await ForgotPassword(email);
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="flex flex-col items-center my-20 gap-y-4">  
            <label htmlFor="email" className="text-2xl font-bold">
                Verify your email in order to reset your password.
            </label>
            <input
                id="email"
                type="email"
                value={email}
                onInput={e => setEmail(e.currentTarget.value)}
                placeholder="your@email.com"
                className="w-1/4 px-3 py-2 rounded border border-indigo-300 focus:border-indigo-700 focus:outline-none"
            />
            <button onClick={handleClick} className="bg-indigo-500 text-indigo-50 px-4 py-1 rounded border border-indigo-800 hover:opacity-80 active:bg-indigo-600">
                Verify
            </button>
        </div>
    );
}

export default ForgotPasswordPage;