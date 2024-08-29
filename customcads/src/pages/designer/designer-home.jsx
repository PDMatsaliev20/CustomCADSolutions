import { useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next'
import RecentItem from '@/components/dashboard/recent-item';
import ErrorPage from '@/components/error-page';

function DesignerHome() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const loaderData = useLoaderData();
    if (loaderData.error) {
        const { unauthenticated, unauthorized } = loaderData;
        if (unauthenticated) {
            return <ErrorPage status={401} />;
        } else if (unauthorized) {
            return <ErrorPage status={403} />;
        } else {
            return <ErrorPage status={400} />;
        }
    }

    const { loadedCads: recentCads, loadedOrders: recentOrders, } = loaderData;   

    return (
        <div className="flex flex-col gap-y-6 my-2">
            <div>
                <h1 className="text-3xl text-center text-indigo-950 font-bold">
                    {tPages('designer.home_title')}
                </h1>
                <hr className="mt-2 border-2 border-indigo-700" />
            </div>
            <div className="flex flex-wrap gap-x-8">
                <div className="basis-1/3 grow flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold">
                        {tPages('designer.home_subtitle_1')}
                    </h2>
                    <ol className="grid grid-rows-5 gap-y-3 border-4 border-indigo-500 pt-3 pb-6 px-8 rounded-2xl bg-indigo-100">
                        <li key={0} className="px-2 pb-2 border-b-2 border-indigo-900 rounded">
                            <div className="flex items-center gap-x-4 font-bold">
                                <span className="basis-1/6">{tCommon('labels.name')}</span>
                                <div className="grow">
                                    <div className="flex justify-between items-center px-4 py-2">
                                        <p className="basis-1/3 text-start">{tCommon('labels.category')}</p>
                                        <p className="basis-1/3 text-center">{tCommon('labels.upload_date')}</p>
                                        <p className="basis-1/3 text-end">{tCommon('labels.status')}</p>
                                    </div>
                                </div>
                            </div>
                        </li>
                        {recentCads.map(cad => <li key={cad.id}>
                            <RecentItem to={`/designer/cads/${cad.id}`} item={{ ...cad, category: cad.category.name, date: cad.creationDate }} />
                        </li>)}
                    </ol>
                </div>
                <div className="min-h-full basis-1/3 grow flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold">
                        {tPages('designer.home_subtitle_2')}
                    </h2>
                    <ol className="grid grid-rows-5 gap-y-3 border-4 border-indigo-500 pt-3 pb-6 px-8 rounded-2xl bg-indigo-100">
                        <li key={0} className="px-2 pb-2 border-b-2 border-indigo-900 rounded">
                            <div className="flex items-center gap-x-4 font-bold">
                                <span className="basis-1/6">{tCommon('labels.name')}</span>
                                <div className="grow">
                                    <div className="flex justify-between items-center px-4 py-2">
                                        <p className="basis-1/3 text-start">{tCommon('labels.category')}</p>
                                        <p className="basis-1/3 text-center">{tCommon('labels.upload_date')}</p>
                                        <p className="basis-1/3 text-end">{tCommon('labels.status')}</p>
                                    </div>
                                </div>
                            </div>
                        </li>
                        {recentOrders.map(order => <li key={order.id}>
                            <RecentItem to={`/designer/cads/${order.cadId}`} item={{ ...order, date: order.orderDate }} />
                        </li>)}
                    </ol>
                </div>
            </div>
        </div>
    );
}

export default DesignerHome;