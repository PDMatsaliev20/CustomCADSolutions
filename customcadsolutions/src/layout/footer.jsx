import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTwitter, faInstagram, faFacebook, faGithub } from '@fortawesome/free-brands-svg-icons'

function Footer() {
    const { t } = useTranslation();

    return (
        <footer className="basis-full grow sticky z-50 self-end py-4 border-t-2 border-indigo-300 rounded-t-lg bg-indigo-100 ">
            <div className="flex justify-evenly items-center">
                <section className="flex gap-6 underline-offset-4 underline font-semibold text-sm">
                    <p className="text-center"><Link to="/policy">{t('Privacy Policy')}</Link></p>
                    <p className="text-center"><Link to="/about">{t('About Us')}</Link></p>
                </section>
                <section>
                    <p className="text-lg font-bold">
                        &copy; 2023-{new Date().getFullYear()} -
                        <Link to="/" className="font-black"> CustomCADSolutions</Link>
                    </p>
                </section>
                <section className="italic flex gap-x-3">
                    <span>{t('Contacts')}</span>
                    <div className="inline-flex gap-x-2">
                        <a href="https://www.instagram.com/customcadsolutions/">
                            <FontAwesomeIcon icon={faInstagram} />
                        </a>
                        <a href="https://twitter.com/customcads/">
                            <FontAwesomeIcon icon={faTwitter} />
                        </a>
                        <a href="#"> {/* Create a Facebook profile */}
                            <FontAwesomeIcon icon={faFacebook} />
                        </a>
                        <a href="https://github.com/NinjataWRLD/CustomCADSolutions/">
                            <FontAwesomeIcon icon={faGithub} />
                        </a>
                    </div>
                </section>
            </div>
        </footer>
    );
}

export default Footer;