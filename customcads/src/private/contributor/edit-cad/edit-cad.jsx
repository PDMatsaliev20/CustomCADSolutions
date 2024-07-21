import { useTranslation } from 'react-i18next'
import { useParams, useNavigate, useLoaderData } from 'react-router-dom'
import { useState, useEffect } from 'react'
import axios from 'axios'

function EditCadPage() {
    const { t } = useTranslation();
    const { id } = useParams();
    const { loadedCategories, loadedCad } = useLoaderData();
    const navigate = useNavigate();

    const [cad, setCad] = useState({ name: '', description: '', categoryId: 0, price: 0 });
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        setCad({ ...loadedCad, categoryId: loadedCad.category.id.toString() });
    }, []);

    const handleInput = (e) => {
        const { name, value } = e.target;
        const newCad = { ...cad, [name]: value.trim() };
        setCad(cad => ({ ...cad, [name]: value }));

        if (JSON.stringify(loadedCad) !== JSON.stringify(newCad)) {
            setIsEditing(true);
        } else {
            setIsEditing(false);
        }
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();
        const body = { name: cad.name, description: cad.description, categoryId: cad.category.id, price: cad.price };
        const response = await axios.put(`https://localhost:7127/API/Cads/${id}`, body, {
            withCredentials: true
        }).catch(e => console.error(e));

        if ((100 < response.status) && (response.status < 300)) {
            navigate(`/cads`);
        }
    };

    return (
        <div className="flex flex-wrap gap-y-8">
            <div className="basis-full">
                <h1 className="text-4xl text-center text-indigo-950 font-bold">{`CAD #${id}`}</h1>
            </div>
            <div className="basis-10/12 mx-auto text-indigo-100">
                <form onSubmit={handleFormSubmit} autoComplete="off">
                    <div className="flex flex-wrap gap-y-8 px-8 py-4 bg-indigo-500 rounded-md border-2 border-indigo-700 shadow-lg shadow-indigo-900">
                        <header className="basis-full">
                            <div className="flex items-center justify-around">
                                <select name="categoryId" value={cad.categoryId} onChange={handleInput}
                                    className="bg-indigo-200 text-indigo-700 px-3 py-3 rounded-xl font-bold focus:outline-none border-2 border-indigo-400 shadow-lg shadow-indigo-900"
                                >
                                    {
                                        loadedCategories.map(category => <option key={category.id} value={category.id} className="bg-indigo-50" >{t(`common.categories.${category.name}`)}</option>)
                                    }
                                </select>
                                <input
                                    type="text"
                                    name="name"
                                    value={cad.name}
                                    onInput={handleInput}
                                    className="bg-indigo-400 text-3xl text-center font-bold focus:outline-none py-2 rounded-xl border-4 border-indigo-300 shadow-xl shadow-indigo-900"
                                />
                                <label htmlFor="price" className="w-1/6 flex items-center gap-x-2 bg-indigo-200 text-indigo-700 rounded-xl px-4 py-2 italic border-4 border-indigo-300 shadow-md shadow-indigo-950">
                                    <span>{t('body.editCad.Price')}</span>
                                    <input
                                        type="number"
                                        name="price"
                                        value={cad.price}
                                        onInput={handleInput}
                                        className="w-full bg-inherit focus:outline-none"
                                    />
                                </label>
                            </div>
                        </header>
                        <section className="basis-full flex flex-wrap gap-y-1 bg-indigo-100 rounded-xl border-2 border-indigo-700 shadow-lg shadow-indigo-900 px-4 py-4">
                            <label htmlFor="description" className="w-full flex justify-between text-indigo-900 text-lg font-bold">
                                <span>{t('body.editCad.Description')}</span>
                                <sub className="opacity-50 text-indigo-950 font-thin">
                                    {t('body.editCad.you can edit the name, description, category and price')}
                                </sub>
                            </label>
                            <textarea
                                id="description"
                                name="description"
                                onInput={handleInput}
                                value={cad.description}
                                rows={4}
                                maxLength={750}
                                minLength={5}
                                className="w-full h-auto bg-inherit text-indigo-700 focus:outline-none"
                            />
                        </section>
                        <footer className="basis-full flex justify-between">
                            <div className="text-start">
                                <span className="font-semibold">{t('body.editCad.Created by')}</span>
                                <span className="underline underline-offset-4 italic">
                                    {loadedCad.creatorName}
                                </span>
                            </div>
                            <div className="text-end">
                                <span className="font-semibold">{t('body.editCad.Created on')}</span>
                                <time dateTime={loadedCad.creationDate.replaceAll('.', '-')} className="italic">
                                    {loadedCad.creationDate}
                                </time>
                            </div>
                        </footer>
                    </div>
                    <div className={`${isEditing ? 'flex justify-center mt-8' : ' hidden'}`}>
                        <button className="bg-indigo-500 text-indigo-50 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950">
                            {t('body.editCad.Save changes')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default EditCadPage;