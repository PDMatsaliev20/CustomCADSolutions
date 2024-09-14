import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import Tag from '@/components/tag';

function OrderItem({ status, order, onDelete }) {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const [buttons, setButtons] = useState(<></>);
    const [shouldBeDelivered, setShouldBeDelivered] = useState(order.shouldBeDelivered);

    const handleDownload = async () => {
        try {
            const { data, headers: { 'content-type': contentType } } = await DownloadOrderCad(order.id);
            const blob = new Blob([data], { type: contentType });

            const url = window.URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;

            switch (contentType) {
                case 'model/gltf-binary': link.download = `${order.name}.glb`; break;
                case 'application/zip': link.download = `${order.name}.zip`; break;
                default: console.error('Unsupported MIME type.'); return;
            }

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Failed to download the file', error);
        }
    };
    const handleRequest = async () => {
        try {
            const flipShouldBeDelivered = {
                op: 'replace',
                path: '/shouldBeDelivered',
                value: !shouldBeDelivered
            };
            await PatchOrder(order.id, [flipShouldBeDelivered]);

            if (shouldBeDelivered) {
                alert(tPages('orders.alert_cancel'));
            } else {
                alert(tPages('orders.alert_delivery'));
            }
            setShouldBeDelivered(shouldBeDelivered => !shouldBeDelivered);
        } catch (e) {
            console.error(e);
        }
    };

    useEffect(() => {
        switch (status.toLowerCase()) {
            case 'pending':
                setButtons(<>
                    <Link to={`${order.id}`}
                        className="min-w-full bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {tPages('orders.details')}
                    </Link>
                    <button onClick={onDelete}
                        className="bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50"
                    >
                        {tPages('orders.delete')}
                    </button>
                </>);
                break;

            case 'begun':
                setButtons(<>
                    <Link to={`mailto:${order.designerEmail}`}
                        className="basis-6/12 bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {tPages('orders.contact')}
                    </Link>
                    <button onClick={onDelete}
                        className="basis-5/12 bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50"
                    >
                        {tPages('orders.cancel_order')}
                    </button>
                </>);
                break;

            case 'finished':
                setButtons(<>
                    <button onClick={handleDownload}
                        className="basis-6/12 bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {tPages('orders.download')}
                    </button>
                    <button onClick={handleRequest}
                        className="basis-5/12 bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50"
                    >
                        {
                            shouldBeDelivered
                                ? tPages('orders.cancel_request')
                                : tPages('orders.request')
                        }
                    </button>
                </>);
                break;
        }
    }, [status, tPages]);

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
                    {buttons}
                </ul>
            </div>
        </div>
    );
}

export default OrderItem;