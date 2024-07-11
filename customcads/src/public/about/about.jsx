import Profile from './components/profile'
import { useTranslation } from 'react-i18next'

function AboutUsPage() {

    const { t } = useTranslation();

    const ivcho = {
        img: './src/assets/engineer.jpg',
        name: t('WebDevName'),
        role: t('WebDevRole'),
        desc: t('WebDevInfo')
    };

    const borko = {
        img: './src/assets/designer.jpg',
        name: t('3dDesignerName'),
        role: t('3dDesignerRole'),
        desc: t('3dDesignerInfo')
    };

    return (
        <div className="my-10 bg-indigo-300 px-4 py-1 rounded-md">
            <h1 className="text-4xl text-center font-semibold py-7">{t('About Us and Our Team')}</h1>
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
                        {t('Want to join our team?')}
                    </span>
                    <span>
                        {t("We're looking for (junior) web developers and 3d designers!")}
                    </span>
                    <a href="mailto:customcadsolutions@gmail.com" className="text-sky-300"> {t('Email us HERE')}</a>
                </div>
            </section>
        </div>
    );
}

export default AboutUsPage;