import { useTranslation } from 'react-i18next';
import Cad from '@/components/cads/cad';
import BtnLink from './components/btn-link';

function HomePage() {
    const { t: tPages } = useTranslation('pages');

    return (
        <>
            <section className="my-4 flex justify-between items-center">
                <article className="h-full basis-8/12 shrink-0 flex flex-wrap items-center gap-y-8 text-center">
                    <h1 className="basis-full text-5xl font-bold italic">{tPages('home.title')}</h1>
                    <span className="basis-full flex flex-col gap-y-3">
                        <span className="text-2xl ">{tPages('home.subtitle')}</span>
                        <span className="text-lg italic">{tPages('home.hint')}</span>
                    </span>
                    <div className="basis-full flex justify-center gap-x-5">
                        <BtnLink to="/register/client" text={tPages('home.buy_btn')} />
                        <BtnLink to="/register/contributor" text={tPages('home.sell_btn')} />
                    </div>
                </article>
                <aside className="grow h-96">
                    <Cad isHomeCad />
                </aside>
            </section>
        </>
    );
}

export default HomePage;