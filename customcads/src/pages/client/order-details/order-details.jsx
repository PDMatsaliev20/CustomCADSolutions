import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useParams, useNavigate, useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { PutOrder } from '@/requests/private/orders';
import { dateToMachineReadable } from '@/utils/date-manager';

function OrderDetails() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');

    const navigate = useNavigate();
    const { id, loadedCategories, loadedOrder } = useLoaderData();
    var loadedValues = {
        name: loadedOrder.name,
        description: loadedOrder.description,
        categoryId: loadedOrder.category.id
    };

    const { register, watch, reset, handleSubmit } = useForm({ defaultValues: loadedValues });
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        const [newName, newDescription, newCategoryId] = Object.values(watch());
        const [oldName, oldDescription, oldCategoryId] = Object.values(loadedValues);

        const nameIsChanged = newName.trim() !== oldName;
        const descriptionIsChanged = newDescription.trim() !== oldDescription;
        const categoryIdIsChanged = Number(newCategoryId) !== oldCategoryId;

        setIsEditing(nameIsChanged || descriptionIsChanged || categoryIdIsChanged);
    }, [watch()]);

    const onSubmit = async (data) => {
        try {
            await PutOrder(id, data);
            setIsEditing(false);
            reset(data);
            navigate('');
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="my-2">
            <form onSubmit={handleSubmit(onSubmit)} autoComplete="off">
                <div className="flex flex-wrap gap-y-2 my-2">
                    <div className="basis-full">
                        <div className="flex justify-evenly items-center">
                            <button className={`${isEditing ? '' : 'invisible'} bg-indigo-700 text-indigo-50 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950 hover:bg-indigo-600 active:opacity-90`}>
                                {tPages('orders.save_changes')}
                            </button>
                            <h1 className="text-4xl text-indigo-950 font-bold">{tPages('orders.order-details_title', { id: id })}</h1>
                            <button className={`${isEditing ? '' : 'invisible'} bg-indigo-200 text-indigo-800 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950 hover:bg-indigo-300 active:opacity-80`}
                                type="button"
                                onClick={() => reset(loadedValues)}
                            >
                                {tPages('orders.revert_changes')}
                            </button>
                        </div>
                    </div>
                    <div className="basis-10/12 mx-auto text-indigo-100">
                        <div className="flex flex-wrap gap-y-8 px-8 py-4 bg-indigo-500 rounded-md border-2 border-indigo-700 shadow-lg shadow-indigo-900">
                            <header className="basis-full">
                                <div className="flex items-center justify-around">
                                    <select {...register('categoryId')}
                                        className="bg-indigo-200 text-indigo-700 px-3 py-3 rounded-xl font-bold focus:outline-none border-2 border-indigo-400 shadow-lg shadow-indigo-900"
                                    >
                                        {loadedCategories.map(category =>
                                            <option key={category.id} value={category.id} className="bg-indigo-50">
                                                {tCommon(`categories.${category.name}`)}
                                            </option>)}
                                    </select>
                                    <input {...register('name')}
                                        className="bg-indigo-400 text-3xl text-center font-bold focus:outline-none py-2 rounded-xl border-4 border-indigo-300 shadow-xl shadow-indigo-900"
                                    />
                                    <span className="bg-indigo-200 text-indigo-700 px-4 py-2 rounded-xl italic border-4 border-indigo-300 shadow-md shadow-indigo-950">
                                        {tCommon(`statuses.${loadedOrder.status}`)}
                                    </span>
                                </div>
                            </header>
                            <section className="basis-full flex flex-wrap gap-y-1 bg-indigo-100 rounded-xl border-2 border-indigo-700 shadow-lg shadow-indigo-900 px-4 py-4">
                                <label className="w-full flex justify-between text-indigo-900 text-lg font-bold">
                                    <span>{tPages('orders.description')}</span>
                                    <sub className="opacity-50 text-indigo-950 font-thin">
                                        {tPages('orders.hint')}
                                    </sub>
                                </label>
                                <textarea
                                    rows={4}
                                    {...register('description', { minLength: 5, maxLength: 750 })}
                                    className="w-full h-auto bg-inherit text-indigo-700 focus:outline-none resize-none"
                                />
                            </section>
                            <footer className="basis-full flex justify-between">
                                <div className="text-start">
                                    <span className="font-semibold">{tPages('orders.ordered_by')} </span>
                                    <span className="underline underline-offset-4 italic">
                                        {loadedOrder.buyerName}
                                    </span>
                                </div>
                                <div className="text-end">
                                    <span className="font-semibold">{tPages('orders.ordered_on')}</span>
                                    <time dateTime={dateToMachineReadable(loadedOrder.orderDate)} className="italic">
                                        {loadedOrder.orderDate}
                                    </time>
                                </div>
                            </footer>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    );
}

export default OrderDetails;