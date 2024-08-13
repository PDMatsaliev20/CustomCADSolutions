import { useTranslation } from 'react-i18next';
import Profile from './components/profile';

function AboutUsPage() {
    const { t } = useTranslation();

    const ivcho = {
        img: '/src/assets/engineer.jpg',
        name: t('public.about.web-dev_name'),
        role: t('public.about.web-dev_role'),
        desc: t('public.about.web-dev_info')
    };

    const borko = {
        img: '/src/assets/designer.jpg',
        name: t('public.about.3D-designer_name'),
        role: t('public.about.3D-designer_role'),
        desc: t('public.about.3D-designer_info')
    };

    return (
        <div className="my-6 bg-indigo-300 px-4 rounded-md">
            <h1 className="text-4xl text-center font-semibold py-7">{t('public.about.title')}</h1>
            <section className="mb-5 gap-2 px-5 pt-5 bg-indigo-800 rounded-md">
                <div className="flex flex-col flex-wrap gap-x-3 gap-y-5 xl:flex-row">
                    <article className="basis-[49%] shrink-0">
                        <Profile person={ivcho} />
                    </article>
                    <article className="basis-[49%] shrink-0">
                        <Profile person={borko} />
                    </article>
                </div>
                <div className="flex flex-col py-3 items-center text-indigo-50">
                    <span className="">
                        {t('public.about.want_to_join')}
                    </span>
                    <span>
                        {t("public.about.looking_for")}
                    </span>
                    <a href="mailto:customcadsolutions@gmail.com" className="text-sky-300"> {t('public.about.email')}</a>
                </div>
            </section>
        </div>
    );
}

export default AboutUsPage;