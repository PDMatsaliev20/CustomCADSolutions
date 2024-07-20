import Path from './components/path'
import BtnLink from './components/btn-link'
import Cad from '@/components/cad'
import { useTranslation } from 'react-i18next'

function HomePage() {
    const { t } = useTranslation();

    const customerParent = { path: '/register/client', content: t('body.home.Register1Btn') };
    const customerChildren = [
        { id: 1, path: '/orders/gallery', content: t("body.home.SubRegister1Btn1") },
        { id: 2, path: '/orders/custom', content: t("body.home.SubRegister1Btn2") },
    ];

    const contributorParent = { path: '/register/contributor', content: t('body.home.Register2Btn') };
    const contributorChildren = [
        { id: 3, path: 'cads/upload', content: t("body.home.SubRegister2Btn1") },
        { id: 4, path: 'cads/sell', content: t("body.home.SubRegister2Btn2") },
    ];

    return (
        <>
            <section className="flex justify-between items-center">
                <article className="h-full basis-8/12 shrink-0 flex flex-wrap items-center gap-y-8 text-center">
                    <h1 className="basis-full text-5xl font-bold italic">{t('body.home.Title')}</h1>
                    <span className="basis-full flex flex-col gap-y-3">
                        <span className="text-2xl ">{t('body.home.Subtitle')}</span>
                        <span className="text-lg italic">{t('body.home.Hint')}</span>
                    </span>
                    <div className="basis-full flex justify-center gap-x-5">
                        <BtnLink to="/register/client" text={t('body.home.BuyBtn')} />
                        <BtnLink to="/register/contributor" text={t('body.home.SellBtn')} />
                    </div>
                </article>
                <aside className="grow h-96">
                    <Cad isHomeCad />
                </aside>
            </section>

            <hr className="border-t border-black" />

            <h3 className="text-4xl text-center mt-8 font-semibold">{t('body.home.Title2')}</h3>
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