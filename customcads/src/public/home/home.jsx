import Path from './components/path'
import BtnLink from './components/btn-link'
import Cad from '@/components/cad'
import { useTranslation } from 'react-i18next'

function HomePage() {

    const { t } = useTranslation();

    const customerParent = { path: '/register/client', content: t('Register as Client') };
    const customerChildren = [
        { id: 1, path: '/orders/gallery', content: t("Order from our Gallery") },
        { id: 2, path: '/orders/custom', content: t("Order from our 3D Designers") },
    ];

    const contributorParent = { path: '/register/contributor', content: t('Register as Contributor') };
    const contributorChildren = [
        { id: 3, path: 'cads/upload', content: t("Upload to Gallery") },
        { id: 4, path: 'cads/sell', content: t("Sell Directly to Us") },
    ];

    return (
        <>
            <section className="flex justify-between">
                <article className="basis-8/12 shrink-0 flex flex-col justify-center gap-y-8 text-center">
                    <h1 className="text-5xl font-bold italic">{t('The Land of 3D Models')}</h1>
                    <span className="flex flex-col gap-y-3">
                        <span className="text-2xl ">{t('We offer high-quality 3D Models tailored to your needs')}</span>
                        <span className="text-lg italic">{t('optional 5-10 business day delivery')}</span>
                    </span>
                    <div className="flex justify-center gap-x-5">
                        <BtnLink to="/register/client" text={t('Looking to buy')} />
                        <BtnLink to="/register/contributor" text={t('Looking to sell')} />
                    </div>
                </article>
                <aside className="grow h-96">
                    <Cad isHomeCad />
                </aside>
            </section>

            <hr className="border-t border-black" />
            
            <h3 className="text-4xl text-center mt-8 font-semibold">{t('Two ways to go about this')}</h3>
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