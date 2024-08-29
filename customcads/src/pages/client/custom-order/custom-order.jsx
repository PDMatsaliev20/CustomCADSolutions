import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import { GetCategories } from '@/requests/public/home';
import { PostOrder } from '@/requests/private/orders';
import Input from '@/components/fields/input';
import Select from '@/components/fields/select';
import TextArea from '@/components/fields/textarea';
import validateCustomOrder from './custom-order.validate';

function CustomOrder() {
    const { t: tCommon } = useTranslation('common');
    const { t: tPages } = useTranslation('pages');
    const navigate = useNavigate();
    const [categories, setCategories] = useState([]);
    const {
        values: order,
        touched,
        errors,
        handleBlur,
        handleInput,
        handleCheckboxInput,
        handleSubmit
    } = useForm({ name: '', description: '', categoryId: 0, shouldBeDelivered: false }, validateCustomOrder);

    useEffect(() => {
        fetchCategories();
    }, []);

    const handleSubmitCallback = async () => {
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
            <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                <div className="w-7/12 mx-auto flex flex-wrap items-start gap-x-4 gap-y-4 bg-indigo-700 py-8 px-10 rounded-xl border-4 border-indigo-500 shadow-lg shadow-indigo-700">
                    <div className="basis-1/2 flex flex-wrap">
                        <Input
                            id="name"
                            label={tCommon('labels.name')}
                            name="name"
                            value={order.name}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={tCommon('placeholders.order_name')}
                            className="inline-block w-full min-h-10 px-3 rounded bg-indigo-50 text-indigo-900 border-2 focus:outline-none focus:border-indigo-300"
                            touched={touched.name}
                            error={errors.name}
                            isRequired
                        />
                    </div>
                    <div className="basis-1/3 grow flex flex-wrap text-indigo-50">
                        <Select
                            id="category"
                            label={tCommon('labels.category')}
                            name="categoryId"
                            value={order.category}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            items={categories}
                            defaultOption={tCommon('categories.None')}
                            onMap={categoryMap}
                            className="inline-block w-full min-h-10 rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                            touched={touched.categoryId}
                            error={errors.categoryId}
                            isRequired
                        />
                    </div>
                    <div className="basis-full flex flex-wrap">
                        <TextArea
                            id="description"
                            label={tCommon('labels.description')}
                            isRequired
                            name="description"
                            value={order.description}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300"
                            placeholder={tCommon('placeholders.order_description')}
                            rows={3}
                            touched={touched.description}
                            error={errors.description}
                        />
                    </div>
                    <div className="basis-full flex gap-x-1">
                        <input
                            id="delivery"
                            type="checkbox"
                            name="shouldBeDelivered"
                            checked={order.shouldBeDelivered}
                            onChange={handleCheckboxInput}
                            onBlur={handleBlur}
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