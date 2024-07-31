import useForm from '@/hooks/useForm'
import validateCustomOrder from './validate-custom-order'
import orderValidation from '@/constants/data/order'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import axios from 'axios'

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

    const createOrder = async () => {
        await axios.post('https://localhost:7127/API/Orders', order, {
            withCredentials: true
        }).catch(e => console.error(e));
    }

    const handleSubmitCallback = () => {
        createOrder();
        navigate("/orders");
    };

    return (
        <div className="flex flex-col gap-y-8 my-6">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">{t('body.customOrder.Title')}</h1>
            <form onSubmit={(e) => handleSubmit(e, handleSubmitCallback)} autoComplete="off" noValidate>
                <div className="w-1/2 mx-auto flex flex-wrap justify-center gap-y-4 gap-x-4 bg-indigo-700 py-6 px-12 rounded-xl">
                    <div className="basis-full flex flex-wrap">
                        <label htmlFor="name" className="basis-full text-indigo-50">
                            {t('common.labels.Name')}
                        </label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={order.name}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                            placeholder={t("body.customOrder.NamePlaceholder")}
                            maxLength={orderValidation.name.maxLength}
                        />
                        <span className={`${touched.name && errors.name ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                            {errors.name}
                        </span>
                    </div>
                    <div className="basis-full flex flex-wrap">
                        <label htmlFor="description" className="basis-full text-indigo-50">
                            {t('common.labels.Description')}
                        </label>
                        <textarea
                            id="description"
                            name="description"
                            value={order.description}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                            placeholder={t('body.customOrder.DescriptionPlaceholder')}
                        />
                        <span className={`${touched.description && errors.description ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                            {errors.description}
                        </span>
                    </div>
                    <div className="basis-full flex flex-wrap">
                        <label htmlFor="category" className="basis-full text-indigo-50">
                            {t('common.labels.Category')}
                        </label>
                        <select
                            id="category"
                            name="categoryId"
                            value={order.category}
                            onInput={handleInput}
                            onBlur={handleBlur}
                            className="inline-block w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
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
                    <div className="mt-1">
                        <button className="bg-indigo-200 text-indigo-800 rounded py-2 px-4">
                            {t('body.customOrder.Order')}
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );

    async function fetchCategories() {
        const response = await axios.get('https://localhost:7127/API/Common/Categories');
        const categories = response.data;
        setCategories(categories);
    };
}

export default CustomOrder;