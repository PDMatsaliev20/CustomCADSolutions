import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function RecentItem({ to, item }) {
    const { t: tCommon } = useTranslation('common');

    return (
        <Link to={to} className="flex items-center gap-x-4 group/row hover:no-underline">
            <span className="basis-1/6 max-w-40 truncate text-lg font-extrabold underline-offset-4 group-hover/row:underline group-hover/row:text-xl">
                {item.name}
            </span>
            <div className="grow group-hover/row:text-lg group-hover/row:font-bold">
                <div className="flex justify-between items-center px-4 py-2">
                    <p>{tCommon(`categories.${item.category.name}`)}</p>
                    <p>{item.date}</p>
                    <p>{tCommon(`statuses.${item.status}`)}</p>
                </div>
            </div>
        </Link>
    );
}

export default RecentItem;