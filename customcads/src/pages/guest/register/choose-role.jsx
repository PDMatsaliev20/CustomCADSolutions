import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function ChooseRole() {
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');
    const navigate = useNavigate();

    const [role, setRole] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        navigate(`${role}`);
    };

    const handleClient = () => setRole('client');
    const handleContributor = () => setRole('contributor');

    return (
        <section className="my-8 mx-auto w-7/12 pt-16 pb-12 bg-indigo-200 border-2 border-indigo-400 rounded-2xl shadow-xl shadow-indigo-300">
            <h2 className="font-bold text-4xl text-center">{tPages('register.choose-role_title')}</h2>
            <div className="mt-10 flex justify-center">
                <form onSubmit={handleSubmit} >
                    <div className="flex flex-wrap mb-6">
                        <div className="flex items-center">
                            <input
                                id="client"
                                type="radio"
                                name="role"
                                value="client"
                                onClick={handleClient}
                                className="w-5 aspect-square"
                                required
                            />
                        </div>
                        <div className="ms-2">
                            <label htmlFor="client" className="text-lg font-bold">
                                {tPages('register.client_option')}
                                <strong className="text-indigo-800"> {tCommon('roles.Client')}</strong>
                                <p className="text-sm font-normal">{tPages('register.client_description')}</p>
                            </label>
                        </div>
                    </div>
                    <div className="flex flex-wrap">
                        <div className="flex items-center">
                            <input
                                id="contributor"
                                type="radio"
                                name="role"
                                value="contributor"
                                onClick={handleContributor}
                                className="w-5 aspect-square"
                                required
                            />
                        </div>
                        <div className="ms-2">
                            <label className="text-lg font-bold" htmlFor="contributor">
                                {tPages('register.contributor_option')}
                                <strong className="text-indigo-800"> {tCommon('roles.Contributor')}</strong>
                                <p className="text-sm font-normal">{tPages('register.contributor_description')}</p>
                            </label>
                        </div>
                    </div>
                    <div className="flex justify-center mt-10">
                        <button type="submit" className="bg-indigo-300 p-3 rounded-md shadow-md shadow-indigo-400 hover:opacity-70 active:opacity-90">
                            {tPages('register.continue')}
                        </button>
                    </div>
                </form>
            </div>
        </section>
    );
}

export default ChooseRole;