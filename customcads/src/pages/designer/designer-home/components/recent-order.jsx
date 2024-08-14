import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function RecentOrder({ order }) {
    const { t } = useTranslation();

    return (
        <Link to={`/designer/cads/${order.cadId}`} className="flex items-center gap-x-4 group/row hover:no-underline">
            <span className="basis-1/6 max-w-40 truncate text-lg font-extrabold underline-offset-4 group-hover/row:underline">{order.name}</span>
            <div className="grow group-hover/row:italic group-hover/row:font-bold">
                <div className="flex justify-between items-center px-4 py-2">
                    <p>{t(`common.categories.${order.category}`)}</p>
                    <p>{order.orderDate}</p>
                    <p>{t(`common.statuses.${order.status}`)}</p>
                </div>
            </div>
        </Link>
    );
}

export default RecentOrder;