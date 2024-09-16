import { useTranslation } from 'react-i18next';
import Tag from '@/components/tag';

function Order({ order, buttons }) {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');

    return (
        <div className="flex h-48 items-center gap-x-4 p-2 bg-indigo-100 border-2 border-indigo-700 shadow-md shadow-indigo-500 rounded-lg">
            <div className="basis-[15%] shrink-0 aspect-square flex items-center border-2 border-indigo-300 rounded-2xl overflow-hidden">
                <img src={order.imagePath} className="object-cover w-full h-full" />
            </div>
            <div className="grow min-h-full flex flex-wrap items-between">
                <div className="basis-full flex items-end gap-x-2">
                    <h3 className="text-3xl text-indigo-950 font-semibold">{order.name}: </h3>
                    <p className="text-2xl line-clamp-1">{order.description}</p>
                </div>
                <div className="basis-full self-center flex gap-x-2">
                    <Tag tag="delivery" label={tPages('orders.to_be_delivered')} hidden={!order.shouldBeDelivered} />
                    <Tag tag="category" label={tCommon(`categories.${order.category.name}`)} />
                    <Tag tag="date" label={order.orderDate} />
                </div>
            </div>
            <div className="basis-[33%] shrink-0">
                <ul className="flex flex-col gap-y-4 me-10 text-indigo-50 font-bold">
                    {buttons.map((btn, i) => <li key={i} className="basis-full flex flex-col">{btn}</li>)}
                </ul>
            </div>
        </div>
    );
}

export default Order;