import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useForm from '@/hooks/useForm';
import { GetCategories } from '@/requests/public/home';
import { PostOrder } from '@/requests/private/orders';
import Input from '@/components/input';
import Select from '@/components/select';
import TextArea from '@/components/textarea';
import validateCustomOrder from './custom-order.validate';

function CustomOrder() {
    const { t } = useTranslation();
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
            navigate("/orders/pending");
        } catch (e) {
            console.error(e);
        }
    };

    const categoryMap = (category) =>
        <option key={category.id} value={category.id}>
            {t(`common.categories.${category.name}`)}
        </option>;

    return (
        <div className="flex flex-col gap-y-8 mt-4">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">{t('body.customOrder.Title')}</h1>
            <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                <div className="w-7/12 mx-auto flex flex-wrap items-start gap-x-4 gap-y-4 bg-indigo-700 py-8 px-10 rounded-xl">
                    <div className="basis-1/2 flex flex-wrap">
                        <Input
                            id="name"
                            label={t('common.labels.Name')}
                            name="name"
                            value={order.name}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            placeholder={t('body.customOrder.NamePlaceholder')}
                            className="inline-block w-full min-h-10 rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                            touched={touched.name}
                            error={errors.name}
                            isRequired
                        />
                    </div>
                    <div className="basis-1/3 grow flex flex-wrap text-indigo-50">
                        <Select
                            id="category"
                            label={t('common.labels.Category')}
                            name="categoryId"
                            value={order.category}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            items={categories}
                            defaultOption={t('common.categories.None')}
                            onMap={categoryMap}
                            className="inline-block w-full min-h-10 rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                            touched={touched.categoryId}
                            error={errors.categoryId}
                            isRequired
                        />
                    </div>
                    <div className="basis-full flex flex-wrap">
                        <TextArea
                            id="description"
                            label={t('common.labels.Description')}
                            isRequired
                            name="description"
                            value={order.description}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                            placeholder={t('body.customOrder.DescriptionPlaceholder')}
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
                            {t('common.labels.Delivery')}
                        </label>
                    </div>
                    <div className="basis-full flex justify-center">
                        <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-8">
                            {t('body.customOrder.Order')}
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

export default CustomOrder;