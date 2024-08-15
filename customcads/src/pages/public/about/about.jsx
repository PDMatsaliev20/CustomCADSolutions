import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import Profile from './components/profile';

function AboutUsPage() {
    const { t } = useTranslation();

    const ivcho = {
        img: '/src/assets/engineer.jpg',
        name: t('public.about.web-dev_name'),
        role: t('public.about.web-dev_role'),
        desc: t('public.about.web-dev_info'),
        contact: 'ivanangelov414@gmail.com'
    };

    const borko = {
        img: '/src/assets/designer.jpg',
        name: t('public.about.3D-designer_name'),
        role: t('public.about.3D-designer_role'),
        desc: t('public.about.3D-designer_info'),
        contact: '+359884874113'
    };

    return (
        <div className="my-6 bg-indigo-300 px-4 rounded-md pb-0 border-2 border-indigo-800 shadow-2xl shadow-indigo-400">
            <h1 className="text-4xl text-center font-semibold py-7">{t('public.about.title')}</h1>
            <section className="gap-2 px-5 pt-5 bg-indigo-800 rounded-md">
                <div className="flex flex-col flex-wrap gap-x-3 gap-y-5 xl:flex-row">
                    <article className="basis-full shrink-0">
                        <Profile person={ivcho} />
                    </article>
                    <article className="basis-full shrink-0">
                        <Profile person={borko} />
                    </article>
                </div>
                <div className="flex flex-col py-3 items-center text-indigo-50">
                    <span>
                        <span className="italic">{t('public.about.want_to_join')}</span>
                        <Link target="_blank" to="mailto:customcadsolutions@gmail.com" className="text-sky-300"> {t('public.about.email')}</Link>
                    </span>
                </div>
            </section>
        </div>
    );
}

export default AboutUsPage;