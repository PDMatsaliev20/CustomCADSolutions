import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import useForm from '@/hooks/useForm';
import { GetCategories } from '@/requests/public/home';
import { PostCad } from '@/requests/private/cads';
import cadValidation from '@/constants/data/cad';
import validateUploadCad from './upload-cad.validate';

function UploadCad() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [categories, setCategories] = useState([]);

    const {
        values: cad,
        touched,
        errors,
        handleBlur,
        handleInput,
        handleFileUpload,
        handleSubmit
    } = useForm({ name: '', description: '', categoryId: 0, price: 0, file: null, image: null }, validateUploadCad);

    useEffect(() => {
        fetchCategories();
    }, []);

    const handleSubmitCallback = async () => {
        try {
            await PostCad(cad);
            navigate("/cads");
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="my-2">
            <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                <div className="flex flex-col gap-y-4">
                    <div className="flex justify-center items-center gap-x-4">
                        <h1 className="text-4xl text-center text-indigo-950 font-bold">{t('body.uploadCad.Title')}*:</h1>
                        <div className="flex flex-wrap items-center gap-x-2 h-full">
                            <label htmlFor="file" className="flex gap-x-4 bg-indigo-200 rounded-xl py-3 px-4 border-2 border-indigo-500 shadow-md shadow-indigo-700">
                                <FontAwesomeIcon icon="arrow-up-from-bracket" className="text-3xl text-indigo-800" />
                                <div className={`${cad.file ? 'text-indigo-800 font-bold flex items-center' : 'hidden'}`}>
                                    <span>{cad.file && cad.file.name}</span>
                                </div>
                            </label>
                            <input
                                type="file"
                                accept=".glb,.zip"
                                id="file"
                                name="file"
                                onInput={handleFileUpload}
                                onClick={(e) => e}
                                onChange={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                hidden
                            />
                        </div>
                    </div>
                    <div className="w-8/12 mx-auto flex flex-wrap gap-y-4 gap-x-8 bg-indigo-700 py-8 px-12 rounded-xl">
                        <div className="basis-8/12 flex flex-wrap">
                            <label htmlFor="name" className="basis-full text-indigo-50">
                                {t('common.labels.Name')}*
                            </label>
                            <input
                                type="text"
                                id="name"
                                name="name"
                                value={cad.name}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                placeholder={t("body.uploadCad.NamePlaceholder")}
                                maxLength={cadValidation.name.maxLength}
                            />
                            <span className={`${touched.name && errors.name ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                                {errors.name}
                            </span>
                        </div>
                        <div className="w-2/12 flex flex-wrap">
                            <div className="basis-full">
                                <label htmlFor="image">
                                    <p className="text-indigo-50 text-center">{t('common.labels.Image')}*</p>
                                    <div className="max-w-1/2 flex justify-center gap-x-4 bg-indigo-200 rounded-xl py-2 px-4 border-2 border-indigo-500 shadow-lg shadow-indigo-900">
                                        <FontAwesomeIcon icon="arrow-up-from-bracket" className="text-2xl text-indigo-800" />
                                        <div className={`${cad.image ? 'text-indigo-800 font-bold flex items-center' : 'hidden'}`}>
                                            <span className="truncate max-w-32">{cad.image && cad.image.name}</span>
                                        </div>
                                    </div>
                                </label>
                                <input
                                    type="file"
                                    accept=".jpg,.png"
                                    id="image"
                                    name="image"
                                    onInput={handleFileUpload}
                                    onChange={handleBlur}
                                    className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                    hidden
                                />
                            </div>
                        </div>
                        <div className="basis-5/12 grow flex flex-wrap items-start gap-y-4">
                            <div className="basis-full">
                                <label htmlFor="category" className="text-indigo-50">
                                    {t('common.labels.Category')}*
                                </label>
                                <select
                                    id="category"
                                    name="categoryId"
                                    value={cad.category}
                                    onInput={handleInput}
                                    onBlur={handleBlur}
                                    className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                >
                                    <option value={''}>{t(`common.categories.None`)}</option>
                                    {categories.map(category =>
                                        <option key={category.id} value={category.id}>{t(`common.categories.${category.name}`)}</option>
                                    )}
                                </select>
                                <span className={`${touched.categoryId && errors.categoryId ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                                    {errors.categoryId}
                                </span>
                            </div>
                        </div>
                        <div className="basis-5/12 grow flex flex-wrap items-start">
                            <label htmlFor="price" className="basis-full text-indigo-50">
                                {t('common.labels.Price')}*
                            </label>
                            <input
                                type="number"
                                id="price"
                                name="price"
                                value={cad.price}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                placeholder={t("body.uploadCad.PricePlaceholder")}
                                max={cadValidation.price.max}
                                min={cadValidation.price.min}
                            />
                            <span className={`${touched.price && errors.price ? 'inline-block' : 'hidden'} z-50 text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                                {errors.price}
                            </span>
                        </div>
                        <div className="basis-full flex flex-wrap">
                            <label htmlFor="description" className="basis-full text-indigo-50">
                                {t('common.labels.Description')}*
                            </label>
                            <textarea
                                id="description"
                                name="description"
                                value={cad.description}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                placeholder={t('body.uploadCad.DescriptionPlaceholder')}
                                maxLength={cadValidation.description.maxLength}
                                rows={3}
                            />
                            <span className={`${touched.description && errors.description ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                                {errors.description}
                            </span>
                        </div>
                        <div className="mt-1 basis-full flex justify-center">
                            <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-6">
                                {t('body.uploadCad.Upload')}
                            </button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    );

    async function fetchCategories() {
        try {
            const { data } = await GetCategories();
            setCategories(data);
        } catch (e) {
            console.error(e);
        }
    };
}

export default UploadCad;