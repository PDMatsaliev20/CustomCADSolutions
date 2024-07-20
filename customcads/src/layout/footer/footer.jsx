import FooterLink from './components/footer-link'
import FooterHeading from './components/footer-heading'
import SocialMedia from './components/social-media'
import { useTranslation } from 'react-i18next'
import { faTwitter, faInstagram, faFacebook, faGithub } from '@fortawesome/free-brands-svg-icons'

function Footer() {
    const { t } = useTranslation();

    return (
        <footer className="justify-self-end py-4 border-t-2 border-indigo-300 rounded-t-lg bg-indigo-100 shadow-indigo-800 shadow-[bg-white_0_0_1em_0]">
            <div className="flex justify-evenly items-center">
                <section className="flex gap-6 underline-offset-4">
                    <FooterLink to="/policy" text={t('footer.Privacy Policy')} />
                    <FooterLink to="/about"  text={t('footer.About Us')} />
                </section>
                <section>
                    <FooterHeading />
                </section>
                <section className="italic flex gap-x-3">
                    <span>{t('footer.Contacts')}</span>
                    <div className="inline-flex gap-x-2">
                        <SocialMedia link="https://www.instagram.com/customcadsolutions" icon={faInstagram} />
                        <SocialMedia link="https://twitter.com/customcads/" icon={faTwitter} />
                        <SocialMedia link="#" icon={faFacebook} />
                        <SocialMedia link="https://github.com/NinjataWRLD/CustomCADSolutions/" icon={faGithub} />
                    </div>
                </section>
            </div>
        </footer>
    );
}

export default Footer;