import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import { GetCategories } from '@/requests/public/home';
import { PostCad } from '@/requests/private/cads';
import { FinishOrder } from '@/requests/private/designer';
import UploadCadBtn from '@/components/fields/upload-cad-btn';
import FileInput from '@/components/fields/file-input';
import Input from '@/components/fields/input';
import TextArea from '@/components/fields/textarea';
import Select from '@/components/fields/select';
import validateUploadCad from './complete-order.validate';

function CompleteOrder() {
    const { t } = useTranslation();
    const { id } = useParams();
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
            const { data } = await PostCad(cad);
            await FinishOrder(id, data.id);
            navigate(`/designer/cads/${data.id}`);
        } catch (e) {
            console.error(e);
        }
    };

    const categoryMap = (category) =>
        <option key={category.id} value={category.id}>
            {t(`common.categories.${category.name}`)}
        </option>;

    return (
        <div className="my-2">
            <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                <div className="flex flex-col gap-y-4">
                    <div className="flex justify-center items-center gap-x-4">
                        <h1 className="text-4xl text-center text-indigo-950 font-bold">{t('private.designer.finish-order_title')}*:</h1>
                        <UploadCadBtn
                            id="file"
                            icon="arrow-up-from-bracket"
                            file={cad.file}
                            accept=".glb,.zip"
                            name="file"
                            onInput={handleFileUpload}
                        />
                    </div>
                    <div className="w-8/12 mx-auto flex flex-wrap gap-y-4 gap-x-8 bg-indigo-700 py-8 px-12 rounded-xl">
                        <div className="basis-8/12">
                            <Input
                                id="name"
                                label={t('common.labels.name')}
                                isRequired
                                name="name"
                                value={cad.name}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                placeholder={t("common.placeholders.cad_name")}
                                touched={touched.name}
                                error={errors.name}
                            />
                        </div>
                        <div className="w-2/12">
                            <FileInput
                                id="image"
                                icon="arrow-up-from-bracket"
                                label={t('common.labels.image')}
                                isRequired
                                file={cad.image}
                                accept=".jpg,.png"
                                name="image"
                                onInput={handleFileUpload}
                            />
                        </div>
                        <div className="basis-5/12 grow text-indigo-50">
                            <Select
                                id="category"
                                label={t('common.labels.category')}
                                isRequired
                                name="categoryId"
                                value={cad.category}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                items={categories}
                                defaultOption={t('common.categories.None')}
                                onMap={categoryMap}
                                touched={touched.categoryId}
                                error={errors.categoryId}
                            />
                        </div>
                        <div className="basis-5/12 grow flex flex-wrap items-start">
                            <Input
                                id="price"
                                label={t('common.labels.price')}
                                isRequired
                                type="number"
                                name="price"
                                value={cad.price}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                placeholder={t("private.designer.PricePlaceholder")}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                touched={touched.price}
                                error={errors.price}
                            />
                        </div>
                        <div className="basis-full flex flex-wrap">
                            <TextArea
                                id="description"
                                label={t('common.labels.description')}
                                isRequired
                                name="description"
                                value={cad.description}
                                onInput={handleInput}
                                onBlur={handleBlur}
                                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                                placeholder={t('common.placeholders.cad_description')}
                                rows={3}
                                touched={touched.description}
                                error={errors.description}
                            />
                        </div>
                        <div className="mt-1 basis-full flex justify-center">
                            <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-6">
                                {t('private.designer.upload')}
                            </button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    );

    async function fetchCategories() {
        const { data } = await GetCategories();
        setCategories(data);
    };
}

export default CompleteOrder;