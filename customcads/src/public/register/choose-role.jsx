import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function ChooseRole() {
    const { t } = useTranslation();
    const navigate = useNavigate();

    const [role, setRole] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        navigate(`${role}`);
    };

    const handleClient = () => setRole('client');
    const handleContributor = () => setRole('contributor');

    return (
        <>
            <section className="my-8 flex justify-center">
                <section className="w-7/12 pt-16 pb-12 bg-indigo-200 rounded-2xl shadow-xl shadow-indigo-300">
                    <h2 className="font-bold text-4xl text-center">{t('body.chooseRole.What are your intentions') }</h2>
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
                                        {t('body.chooseRole.I want to order 3D Models')}
                                        <strong className="text-indigo-800"> {t('common.roles.Client')}</strong>
                                        <p className="text-sm font-normal">{t('body.chooseRole.you can scroll through our gallery or place a custom order')}</p>
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
                                        {t('body.chooseRole.I want to sell 3D Models')}
                                        <strong className="text-indigo-800"> {t('common.roles.Contributor')}</strong>
                                        <p className="text-sm font-normal">{t('body.chooseRole.you can upload to our gallery or sell directly to us')}</p>
                                    </label>
                                </div>
                            </div>
                            <div className="flex justify-center mt-10">
                                <button type="submit" className="bg-indigo-300 p-3 rounded-md shadow-md shadow-indigo-400 opacity-100 hover:opacity-80 focus:opacity-70">
                                    {t('body.chooseRole.Continue to Register')}
                                </button>
                            </div>
                        </form>
                    </div>
                </section>
            </section>
        </>
    );
}

export default ChooseRole;