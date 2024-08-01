import { useTranslation } from 'react-i18next'
import { useParams, useNavigate, useLoaderData } from 'react-router-dom'
import { useState, useEffect } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { PutCad } from '@/requests/private/cads'

function EditCadPage() {
    const { t } = useTranslation();
    const { id } = useParams();
    const { loadedCategories, loadedCad } = useLoaderData();
    const navigate = useNavigate();

    const [cad, setCad] = useState({ name: '', description: '', categoryId: 0, price: 0, image: null });
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        setCad(cad => ({
            ...cad,
            name: loadedCad.name,
            description: loadedCad.description,
            categoryId: loadedCad.category.id,
            price: loadedCad.price,
        }));
    }, []);

    const handleInput = (e) => {
        const { name, value } = e.target;
        const newCad = { ...cad, [name]: value.trim() };
        setCad(cad => ({ ...cad, [name]: value }));

        if (loadedCad.name !== newCad.name
            || loadedCad.description !== newCad.description
            || loadedCad.category.id != newCad.categoryId
            || loadedCad.price != newCad.price) {
            setIsEditing(true);
        } else {
            setIsEditing(false);
        }
    };

    const handleFileUpload = (e) => {
        const { name, files } = e.target;
        setCad(cad => ({ ...cad, [name]: files[0] }));
        if (files[0]) {
            setIsEditing(true);
        } else {
            setIsEditing(false);
        }
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();
        try {
            const body = { ...cad, categoryId: Number(cad.categoryId) };
            const { status } = await PutCad(id, body);
            if ((100 < status) && (status < 300)) {
                navigate(`/cads`);
            }
        } catch (e) {
            console.error(e);
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
                        <footer className="basis-full flex justify-between items-center">
                            <div className="basis-1/3 text-start">
                                <span className="font-semibold">{t('body.editCad.Created by')}</span>
                                <span className="underline underline-offset-4 italic">
                                    {loadedCad.creatorName}
                                </span>
                            </div>
                            <div className="basis-1/3 flex justify-center">
                                <label htmlFor="image" className="flex justify-around gap-x-4 items-center bg-indigo-300 px-4 py-1 rounded-md shadow-lg shadow-indigo-900">
                                    <p className="text-indigo-50 font-bold">{t('common.labels.Image')}</p>
                                    <div className="flex justify-center gap-x-4 bg-indigo-700 rounded-xl py-2 px-4 border-2 border-indigo-400">
                                        <FontAwesomeIcon icon="arrow-up-from-bracket" className="text-xl text-indigo-100" />
                                        <div className={`${cad.image ? 'font-bold flex items-center' : 'hidden'}`}>
                                            <span className="text-indigo-50 w-24 truncate">{cad.image && cad.image.name}</span>
                                        </div>
                                    </div>
                                </label>
                                <input
                                    type="file"
                                    accept=".jpg, .png"
                                    id="image"
                                    name="image"
                                    onInput={handleFileUpload}
                                    hidden
                                />
                            </div>
                            <div className="basis-1/3 text-end">
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