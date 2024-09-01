import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { GetCategories } from '@/requests/public/home';
import { PostOrder } from '@/requests/private/orders';
import Input from '@/components/fields/input';
import Select from '@/components/fields/select';
import TextArea from '@/components/fields/textarea';
import orderValidations from './order-validations';

function CustomOrder() {
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');
    const navigate = useNavigate();
    const [categories, setCategories] = useState([]);
    const [shouldBeDelivered, setShouldBeDelivered] = useState(false);
    const { register, formState, handleSubmit } = useForm({ mode: 'onTouched' });
    const { name,  description, categoryId } = orderValidations();

    useEffect(() => {
        fetchCategories();
    }, []);

    const onSubmit = async (order) => {
        try {
            await PostOrder(order);
            navigate("/client/orders/pending");
        } catch (e) {
            console.error(e);
        }
    };

    const categoryMap = (category) =>
        <option key={category.id} value={category.id}>
            {tCommon(`categories.${category.name}`)}
        </option>;

    return (
        <div className="flex flex-col gap-y-8 mt-2">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">{tPages('orders.custom-order_title')}</h1>
            <form onSubmit={handleSubmit(onSubmit)} noValidate>
                <div className="w-7/12 mx-auto flex flex-wrap items-start gap-x-4 gap-y-4 bg-indigo-700 py-8 px-10 rounded-xl border-4 border-indigo-500 shadow-lg shadow-indigo-700">
                    <div className="basis-1/2 flex flex-wrap">
                        <Input
                            id="name"
                            label={tCommon('labels.name')}
                            rhfProps={register('name', name)}
                            placeholder={tCommon('placeholders.order_name')}
                            className="inline-block w-full min-h-10 px-3 rounded bg-indigo-50 text-indigo-900 border-2 focus:outline-none focus:border-indigo-300"
                            error={formState.errors.name}
                            isRequired
                        />
                    </div>
                    <div className="basis-1/3 grow flex flex-wrap text-indigo-50">
                        <Select
                            id="category"
                            label={tCommon('labels.category')}
                            rhfProps={register('categoryId', categoryId)}
                            items={categories}
                            defaultOption={tCommon('categories.None')}
                            onMap={categoryMap}
                            className="inline-block w-full min-h-10 rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                            error={formState.errors.categoryId}
                            isRequired
                        />
                    </div>
                    <div className="basis-full flex flex-wrap">
                        <TextArea
                            id="description"
                            label={tCommon('labels.description')}
                            rhfProps={register('description', description)}
                            placeholder={tCommon('placeholders.order_description')}
                            rows={3}
                            error={formState.errors.description}
                            className="w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                            isRequired
                        />
                    </div>
                    <div className="basis-full flex gap-x-1">
                        <input
                            id="delivery"
                            type="checkbox"
                            checked={shouldBeDelivered}
                            onChange={() => setShouldBeDelivered(sbd => !sbd)}
                        />
                        <label htmlFor="delivery" className="text-indigo-50 font-bold">
                            {tCommon('labels.delivery')}
                        </label>
                    </div>
                </div>
                <div className="mt-6 basis-full flex justify-center">
                    <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-8 border-2 border-indigo-500">
                        {tPages('orders.order')}
                    </button>
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

export default CustomOrder;