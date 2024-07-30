import { useTranslation } from 'react-i18next'
import { useParams, useNavigate, useLoaderData } from 'react-router-dom'
import { useState, useEffect } from 'react'
import axios from 'axios'

function OrderDetails() {
    const { t } = useTranslation();
    const { id } = useParams();
    const { loadedCategories, loadedOrder } = useLoaderData();
    const navigate = useNavigate();

    const [originalOrder, setOriginalOrder] = useState();
    const [order, setOrder] = useState({ name: '', description: '', categoryId: 0 });
    const [categories, setCategories] = useState([]);
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        if (!categories.length) {
            setCategories(loadedCategories);
        } else {
            const { id } = categories.find(c => c.name === loadedOrder.category);

            const fetchedOrder = { ...loadedOrder, categoryId: id.toString() };

            setOriginalOrder(fetchedOrder);
            setOrder(fetchedOrder);
        }
    }, [categories]);

    const handleInput = (e) => {
        if (order.status !== 'Pending') {
            alert(t('body.orderDetails.Warning'));
            return;
        }
        const newOrder = { ...order, [e.target.name]: e.target.value.trim() };
        setOrder(prevOrder => ({
            ...prevOrder,
            [e.target.name]: e.target.value
        }));

        if (JSON.stringify(originalOrder) !== JSON.stringify(newOrder)) {
                setIsEditing(true);
        } else {
            setIsEditing(false);
        }
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();
        const body = { name: order.name, description: order.description, categoryId: order.categoryId };
        const response = await axios.put(`https://localhost:7127/API/Orders/${id}`, body, {
            withCredentials: true
        }).catch(e => console.error(e));

        if ((100 < response.status) && (response.status < 300)) {
            navigate(`/orders`);
        }
    };

    return (
        <div className="flex flex-wrap gap-y-8">
            <div className="basis-full">
                <h1 className="text-4xl text-center text-indigo-950 font-bold">{`${t('body.orderDetails.Title')} #${id}`}</h1>
            </div>
            <div className="basis-10/12 mx-auto text-indigo-100">
                <form onSubmit={handleFormSubmit} autoComplete="off">
                    <div className="flex flex-wrap gap-y-8 px-8 py-4 bg-indigo-500 rounded-md border-2 border-indigo-700 shadow-lg shadow-indigo-900">
                        <header className="basis-full">
                            <div className="flex items-center justify-around">
                                <select
                                    name="categoryId"
                                    value={order.categoryId}
                                    onChange={handleInput}
                                    className="bg-indigo-200 text-indigo-700 px-3 py-3 rounded-xl font-bold focus:outline-none border-2 border-indigo-400 shadow-lg shadow-indigo-900"
                                >
                                    {categories.map(category =>
                                        <option key={category.id} value={category.id} className="bg-indigo-50">
                                            {t(`common.categories.${category.name}`)}
                                        </option>)}
                                </select>
                                <input
                                    name="name"
                                    value={order.name}
                                    onInput={handleInput}
                                    className="bg-indigo-400 text-3xl text-center font-bold focus:outline-none py-2 rounded-xl border-4 border-indigo-300 shadow-xl shadow-indigo-900"
                                />
                                <span className="bg-indigo-200 text-indigo-700 px-4 py-2 rounded-xl italic border-4 border-indigo-300 shadow-md shadow-indigo-950">
                                    {t(`common.statuses.${loadedOrder.status}`)}
                                </span>
                            </div>
                        </header>
                        <section className="basis-full flex flex-wrap gap-y-1 bg-indigo-100 rounded-xl border-2 border-indigo-700 shadow-lg shadow-indigo-900 px-4 py-4">
                            <label htmlFor="description" className="w-full flex justify-between text-indigo-900 text-lg font-bold">
                                <span>{t('body.orderDetails.Description')}</span>
                                <sub className="opacity-50 text-indigo-950 font-thin">
                                    {t('body.orderDetails.you can edit the name, description and category')}
                                </sub>  
                            </label>
                            <textarea
                                id="description"
                                name="description"
                                onInput={handleInput}
                                value={order.description}
                                rows={4}
                                maxLength={750}
                                minLength={5}
                                className="w-full h-auto bg-inherit text-indigo-700 focus:outline-none"
                            />
                        </section>
                        <footer className="basis-full flex justify-between">
                            <div className="text-start">
                                <span className="font-semibold">{t('body.orderDetails.Ordered by')}</span>
                                <span className="underline underline-offset-4 italic">
                                    {loadedOrder.buyerName}
                                </span>
                            </div>
                            <div className="text-end">
                                <span className="font-semibold">{t('body.orderDetails.Ordered on')}</span>
                                <time dateTime={loadedOrder.orderDate.replaceAll('.', '-')} className="italic">
                                    {loadedOrder.orderDate}
                                </time>
                            </div>
                        </footer>
                    </div>
                    <div className={`${isEditing ? 'flex justify-center mt-8' : ' hidden'}`}>
                        <button className="bg-indigo-500 text-indigo-50 font-bold py-3 px-6 rounded-lg border border-indigo-700 shadow shadow-indigo-950">
                            {t('body.orderDetails.Save changes')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default OrderDetails;