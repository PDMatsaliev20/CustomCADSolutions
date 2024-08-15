import { useTranslation } from 'react-i18next';
import Cad from '@/components/cad';
import BtnLink from './components/btn-link';

function HomePage() {
    const { t } = useTranslation();

    return (
        <>
            <section className="my-4 flex justify-between items-center">
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
                <aside className="grow h-96 rounded-3xl overflow-hidden bg-indigo-100 border-2 border-indigo-500 shadow-md shadow-indigo-700 hover:bg-indigo-200 active:bg-indigo-300">
                    <Cad isHomeCad />
                </aside>
            </section>
        </>
    );
}

export default HomePage;