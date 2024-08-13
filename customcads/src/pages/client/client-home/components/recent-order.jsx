import { Link } from 'react-router-dom'

function RecentOrder({ order }) {
    return (
        <Link to={`/client/orders/${order.status}/${order.id}`} className="flex items-center gap-x-4 group/row hover:no-underline">
            <span className="basis-1/6 max-w-40 truncate text-lg font-extrabold underline-offset-4 group-hover/row:underline">{order.name}</span>
            <div className="grow group-hover/row:italic group-hover/row:font-bold">
                <div className="flex justify-between items-center px-4 py-2 border-2 border-indigo-600">
                    <p>{order.category}</p>
                    <p>{order.orderDate}</p>
                    <p>{order.status}</p>
                </div>
            </div>
        </Link>
    );
}

export default RecentOrder;