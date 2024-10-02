import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import Category from '@/interfaces/category';
import useAuth from '@/hooks/useAuth';
import { GetCategories } from '@/requests/public/categories';
import { PostCad } from '@/requests/private/cads';
import { FinishOrder } from '@/requests/private/designer';
import UploadCadBtn from '@/components/fields/upload-cad-btn';
import FileInput from '@/components/fields/file-input';
import Input from '@/components/fields/input';
import Select from '@/components/fields/select';
import TextArea from '@/components/fields/textarea';
import cadValidations from './cad-validations';

function UploadCad() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const { id } = useParams();
    const navigate = useNavigate();
    const { userRole } = useAuth();
    const [categories, setCategories] = useState([]);
    const [file, setFile] = useState<File | null>(null);
    const [image, setImage] = useState<File | null>();

    interface CadForm {
        name: string
        description: string
        categoryId: string
        price: string
    }

    const { register, formState, handleSubmit } = useForm<CadForm>({ mode: 'onTouched' });
    const { name, description, categoryId, price } = cadValidations();

    useEffect(() => {
        fetchCategories();
    }, []);

    const onSubmit = async (cad: CadForm) => {
        try {
            const { data } = await PostCad({ ...cad, image, file });
            if (userRole === 'Designer') {
                if (id) {
                    await FinishOrder(Number(id), data.id);
                }
                navigate(`/designer/cads/${data.id}`);
            } else {
                navigate('/contributor/cads');
            }
        } catch (e) {
            console.error(e);
        }
    };

    const categoryMap = (category: Category) =>
        <option key={category.id} value={category.id}>
            {tCommon(`categories.${category.name}`)}
        </option>;

    return (
        <div className="my-2">
            <form onSubmit={handleSubmit(onSubmit)} noValidate>
                <div className="flex flex-col gap-y-4">
                    <div className="flex justify-center items-center gap-x-4 ">
                        <h1 className="text-4xl text-center text-indigo-950 font-bold">{tPages('cads.upload-cad_title')}*:</h1>
                        <UploadCadBtn
                            id="file"
                            icon="arrow-up-from-bracket"
                            file={file}
                            accept=".glb,.zip"
                            name="file"
                            onInput={(e) => setFile(e.currentTarget.files?.item(0) ?? null)}
                        />
                    </div>
                    <div className="w-8/12 mx-auto flex flex-wrap gap-y-4 gap-x-8 bg-indigo-700 py-8 px-12 rounded-xl  border-4 border-indigo-500 shadow-md shadow-indigo-700">
                        <div className="basis-full">
                            <Input
                                id="name"
                                label={tCommon('labels.name')}
                                rhfProps={register('name', name)}
                                className="w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                                placeholder={tCommon("placeholders.cad_name")}
                                error={formState.errors.name}
                                isRequired
                            />
                        </div>
                        <div className="basis-5/12 grow text-indigo-50">
                            <Select
                                id="category"
                                label={tCommon('labels.category')}
                                rhfProps={register('categoryId', categoryId)}
                                className="w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                                items={categories}
                                defaultOption={tCommon('categories.None')}
                                onMap={categoryMap}
                                error={formState.errors.categoryId}
                                isRequired
                            />
                        </div>
                        <div className="basis-5/12 grow flex flex-wrap items-start">
                            <Input
                                id="price"
                                label={tCommon('labels.price')}
                                type="number"
                                rhfProps={register('price', price)}
                                placeholder={tCommon("placeholders.cad_price")}
                                className="w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                                error={formState.errors.price}
                                isRequired
                            />
                        </div>
                        <div className="basis-full flex flex-wrap">
                            <TextArea
                                id="description"
                                label={tCommon('labels.description')}
                                rhfProps={register('description', description)}
                                placeholder={tCommon('placeholders.cad_description')}
                                rows={3}
                                error={formState.errors.description}
                                isRequired
                            />
                        </div>
                        <div className="basis-full flex justify-center">
                            <FileInput
                                id="image"
                                icon="arrow-up-from-bracket"
                                label={tCommon('labels.image')}
                                isRequired
                                file={image}
                                accept=".jpg,.png"
                                name="image"
                                onInput={(e) => setImage(e.currentTarget.files?.item(0) ?? null)}
                            />
                        </div>
                    </div>
                    <div className="mt-1 basis-full flex justify-center">
                        <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-6 border-2 border-indigo-500">
                            {tPages('cads.upload')}
                        </button>
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