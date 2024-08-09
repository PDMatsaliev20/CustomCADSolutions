import { useState, useEffect } from 'react';
import { useParams, useNavigate, useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { PutCad, PatchCad, DeleteCad } from '@/requests/private/cads';
import constants from '@/constants/data/cad';
import Cad from '@/components/cad';

function EditCadPage() {
    const { t } = useTranslation();
    const { id } = useParams();
    const { loadedCategories, loadedCad } = useLoaderData();
    const navigate = useNavigate();
    const [cad, setCad] = useState({ name: '', description: '', categoryId: 0, price: 0, image: null });
    const [isEditing, setIsEditing] = useState(false);
    const [isChanged, setIsChanged] = useState(false);

    useEffect(() => {
        setCad(cad => ({
            ...cad,
            name: loadedCad.name,
            description: loadedCad.description,
            categoryId: loadedCad.category.id,
            price: loadedCad.price,
        }));

        const flipIsChanged = () => {
            setIsChanged(true);
        };
        window.addEventListener('PositionChanged', flipIsChanged);

        const sendTrackChangesEvent = () => window.dispatchEvent(new CustomEvent("TrackChanges"));

        const savePosition = (e) => {
            const { coords, panCoords } = e.detail;

            try {
                const updateCoords = {
                    path: "/coords",
                    op: "replace",
                    value: coords
                }, updatePanCoords = {
                    path: "/panCoords",
                    op: "replace",
                    value: panCoords
                }
                PatchCad(id, [updateCoords, updatePanCoords]);
                setIsChanged(false);

                setTimeout(sendTrackChangesEvent, 1500);
            } catch (e) {
                console.error(e);
            }
        };
        window.addEventListener('SavePosition', savePosition);

        return () => {
            window.removeEventListener('PositionChanged', savePosition);
            window.removeEventListener('SavePosition', savePosition);
            clearTimeout(sendTrackChangesEvent);
        };
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
            await PutCad(id, { ...cad, categoryId: Number(cad.categoryId) });
            setIsEditing(false);
            navigate('');
        } catch (e) {
            console.error(e);
        }
    };

    const handleDelete = async () => {
        if (confirm(t('body.cads.Confirmation'))) {
            try {
                await DeleteCad(id);
                navigate('/cads');
            } catch (e) {
                console.error(e);
            }
        }
    };

    return (
        <form onSubmit={handleFormSubmit} autoComplete="off">
            <div className="mb-4 relative">
                <div className={`${isChanged ? 'absolute top-0 left-0' : ' hidden'}`}>
                    <button type="button" onClick={() => window.dispatchEvent(new CustomEvent('SendPosition'))}
                        className="bg-indigo-500 text-indigo-50 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950"
                    >
                        {t('Update Position')}
                    </button>
                </div>
                <div className="flex justify-center items-center gap-x-8">
                    <h1 className="text-4xl text-center text-indigo-950 font-bold">{`CAD #${id}`}</h1>
                </div>
                <div className={`${isEditing ? 'absolute top-0 right-0' : ' hidden'}`}>
                    <button type="submit"
                        className="bg-indigo-500 text-indigo-50 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950"
                    >
                        {t('body.editCad.Save changes')}
                    </button>
                </div>
            </div>
            <div className="flex bg-indigo-300 rounded-md overflow-hidden border-4 border-indigo-700 shadow-lg shadow-indigo-400">
                <div className="flex justify-center items-center px-8">
                    <div className="bg-indigo-200 w-80 h-80 rounded-xl">
                        <Cad cad={loadedCad} />
                    </div>
                </div>
                <div className="grow bg-indigo-500 text-indigo-50 flex flex-col">
                    <header className="flex gap-x-2 px-4 py-4 text-center text-xl font-bold">
                        <select
                            name="categoryId"
                            value={cad.categoryId}
                            onChange={handleInput}
                            className="bg-indigo-200 text-indigo-700 px-3 py-3 rounded-xl font-bold focus:outline-none border-2 border-indigo-400 shadow-lg shadow-indigo-900"
                        >
                            {loadedCategories.map(category =>
                                <option key={category.id} value={category.id} className="bg-indigo-50" >
                                    {t(`common.categories.${category.name}`)}
                                </option>)}
                        </select>
                        <input
                            type="text"
                            name="name"
                            value={cad.name}
                            onInput={handleInput}
                            className="grow bg-indigo-200 text-indigo-700 text-3xl text-center fontextrabold focus:outline-none py-2 rounded-xl border-4 border-indigo-400 shadow-lg shadow-indigo-900"
                            maxLength={constants.name.maxLength}
                        />
                        <label htmlFor="price" className="basis-2/12 flex gap-x-1 items-center bg-indigo-200 text-indigo-700 rounded-xl px-2 py-2 border-4 border-indigo-300 shadow-md shadow-indigo-950">
                            <input
                                id="price"
                                type="number"
                                name="price"
                                value={cad.price}
                                onInput={handleInput}
                                className="hide-spinner w-full text-end bg-inherit focus:outline-none"
                                max={9999}
                            />$
                        </label>
                    </header>
                    <hr className="border-t-2 border-indigo-700" />
                    <section className="m-4 flex flex-wrap gap-y-1 bg-indigo-200 rounded-xl border-2 border-indigo-700 shadow-lg shadow-indigo-900 px-4 py-4">
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
                            rows={5}
                            maxLength={750}
                            minLength={5}
                            className="w-full h-auto bg-inherit text-indigo-700 focus:outline-none"
                        />
                    </section>
                    <hr className="border-t-4 border-indigo-700" />
                    <footer className="px-4 py-3 basis-full flex justify-between items-center">
                        <div className="flex justify-center">
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
                        <div>
                            <span className="font-semibold">{t('body.editCad.Created on')}</span>
                            <time dateTime={loadedCad.creationDate.replaceAll('.', '-')} className="italic">
                                {loadedCad.creationDate}
                            </time>
                        </div>
                        <button
                            type="button"
                            onClick={handleDelete}
                            className="basis-2/12 text-indigo-950 bg-indigo-200 py-2 rounded-md hover:text-indigo-50 hover:bg-red-500 border-2 border-indigo-700"
                        >
                            {t('body.cads.Delete')}
                        </button>
                    </footer>
                </div>
            </div>
        </form>
    );
}

export default EditCadPage;