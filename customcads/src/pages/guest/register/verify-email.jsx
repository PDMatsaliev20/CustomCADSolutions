import { useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { ResendEmailVerification } from '@/requests/public/identity';
import ErrorPage from '@/components/error-page';

function VerifyEmailPage() {
    const { t: tPages } = useTranslation('pages');
    const { username, isEmailConfirmed, doesUserExist } = useLoaderData();

    if (!doesUserExist || isEmailConfirmed) {
        return <ErrorPage status={400} />;
    }

    const sendVerificationEmail = async () => {
        try {
            await ResendEmailVerification(username);
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="my-20 flex flex-col gap-y-4">
            <h1 className="text-3xl text-center font-bold">
                {tPages('register.verify-title')}
            </h1>
            <h3 className="text-xl text-center">
                <span>{tPages('register.when_verified')} </span>
                <button onClick={() => window.location.reload(false)} className="hover:underline">{tPages('register.refresh_page')}</button>.
            </h3>
            <p className="text-center">
                <span>{tPages('register.no_email')}? </span>
                <button
                    onClick={sendVerificationEmail}
                    className="bg-indigo-200 ms-2 px-2 py-1 rounded border-2 border-indigo-400 hover:bg-indigo-300 active:opacity-80"
                >
                    {tPages('register.send_another')}
                </button>
            </p>
        </div>
    );
}

export default VerifyEmailPage;