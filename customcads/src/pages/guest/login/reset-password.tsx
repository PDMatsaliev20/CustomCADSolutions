import { useState } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { ResetPassword } from '@/requests/public/identity';

function ResetPasswordPage() {
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [success, setSuccess] = useState(false);

    const [searchParams] = useSearchParams();
    const token = searchParams.get('token');
    const email = searchParams.get('email');
    console.log(token)
    const handleClick = async () => {
        try {
            if (password === confirmPassword) {
                await ResetPassword(email!, token!, password);
                setSuccess(true);
            } else {
                alert('Passwords do not match.');
            }
        } catch (e) {
            console.error(e);
            alert('Expired token');
        }
    };

    return (
        <div className="flex flex-col items-center my-20 gap-y-4">
            <label className="text-2xl font-bold">
                Reset your password.
            </label>
            <input
                id="password"
                type="password"
                value={password}
                onInput={e => setPassword(e.currentTarget.value)}
                className="w-1/4 px-3 py-2 rounded border border-indigo-300 focus:border-indigo-700 focus:outline-none"
                disabled={success}
            />
            <input
                id="confirmPassword"
                type="password"
                value={confirmPassword}
                onInput={e => setConfirmPassword(e.currentTarget.value)}
                className="w-1/4 px-3 py-2 rounded border border-indigo-300 focus:border-indigo-700 focus:outline-none"
                disabled={success}
            />
            <button onClick={handleClick} className="bg-indigo-500 text-indigo-50 px-4 py-1 rounded border border-indigo-800 hover:opacity-80 active:bg-indigo-600" disabled={success}>
                Verify
            </button>
            {success &&
                <p>All done! Try logging in again {' '}
                    <Link to="/login" className="font-bold">HERE</Link>
                </p>
            }
        </div>
    );
}

export default ResetPasswordPage;