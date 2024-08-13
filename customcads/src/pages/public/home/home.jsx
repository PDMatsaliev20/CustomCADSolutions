import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import AuthContext from '@/contexts/auth-context';
import Cad from '@/components/cad';
import Path from './components/path';
import BtnLink from './components/btn-link';

function HomePage() {
    const { t } = useTranslation();
    const { isAuthenticated, userRole } = useContext(AuthContext);

    if (isAuthenticated) {
        return <Navigate to={`/${userRole.toLowerCase()}`} />;
    }

    const customerParent = { path: '/register/client', content: t('common.roles.Client') };
    const customerChildren = [
        { id: 1, path: '/orders/gallery', content: t("public.home.client_action_1") },
        { id: 2, path: '/orders/custom', content: t("public.home.client_action_2") },
    ];

    const contributorParent = { path: '/register/contributor', content: t('common.roles.Contributor') };
    const contributorChildren = [
        { id: 3, path: 'cads/upload', content: t("public.home.contributor_action_1") },
        { id: 4, path: 'cads/sell', content: t("public.home.contributor_action_2") },
    ];

    return (
        <>
            <section className="flex justify-between items-center">
                <article className="h-full basis-8/12 shrink-0 flex flex-wrap items-center gap-y-8 text-center">
                    <h1 className="basis-full text-5xl font-bold italic">{t('public.home.title')}</h1>
                    <span className="basis-full flex flex-col gap-y-3">
                        <span className="text-2xl ">{t('public.home.subtitle')}</span>
                        <span className="text-lg italic">{t('public.home.hint')}</span>
                    </span>
                    <div className="basis-full flex justify-center gap-x-5">
                        <BtnLink to="/register/client" text={t('public.home.buy_btn')} />
                        <BtnLink to="/register/contributor" text={t('public.home.sell_btn')} />
                    </div>
                </article>
                <aside className="grow h-96">
                    <Cad isHomeCad />
                </aside>
            </section>

            <hr className="border-t border-black" />

            <h3 className="text-4xl text-center mt-8 font-semibold">{t('public.home.title_2')}</h3>
            <section className="my-10">
                <article className="flex justify-evenly items-center gap-5">
                    <Path parent={customerParent} children={customerChildren} />
                    <div className="border-x-2 border-indigo-700 w-3 h-32"></div>
                    <Path parent={contributorParent} children={contributorChildren} />
                </article>
            </section>
        </>
    );
}

export default HomePage;