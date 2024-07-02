import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ClientNavigationalMenu({ shouldBlur, shouldHide }) {
    const { t } = useTranslation();

    const handleClick = () => {
        if (shouldBlur) {
            alert(t('Only for Clients!'));
        }
    }

    return (
        <ul className={`${shouldHide ? "hidden" : ''} ${shouldBlur ? "blur-sm" : ''}`} onClick={handleClick}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>{t('Your Orders')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('Order Custom 3D Model')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('Order from Gallery')}</span>
                        </li>
                    </>
                    :
                    <>
                        <li className="float-left me-4">
                            <Link to="/orders">{t('Your Orders')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/custom">{t('Order Custom 3D Model')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/gallery">{t('Order from Gallery')}</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ClientNavigationalMenu;