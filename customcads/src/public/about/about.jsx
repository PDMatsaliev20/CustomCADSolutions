import { useTranslation } from 'react-i18next';
import Profile from './components/profile';

function AboutUsPage() {

    const { t } = useTranslation();

    const ivcho = {
        img: '/src/assets/engineer.jpg',
        name: t('body.about.WebDevName'),
        role: t('body.about.WebDevRole'),
        desc: t('body.about.WebDevInfo')
    };

    const borko = {
        img: '/src/assets/designer.jpg',
        name: t('body.about.3dDesignerName'),
        role: t('body.about.3dDesignerRole'),
        desc: t('body.about.3dDesignerInfo')
    };

    return (
        <div className="my-6 bg-indigo-300 px-4 rounded-md">
            <h1 className="text-4xl text-center font-semibold py-7">{t('body.about.About Us and Our Team')}</h1>
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
                        {t('body.about.Want to join our team?')}
                    </span>
                    <span>
                        {t("body.about.We're looking for (junior) web developers and 3d designers!")}
                    </span>
                    <a href="mailto:customcadsolutions@gmail.com" className="text-sky-300"> {t('body.about.Email us HERE')}</a>
                </div>
            </section>
        </div>
    );
}

export default AboutUsPage;