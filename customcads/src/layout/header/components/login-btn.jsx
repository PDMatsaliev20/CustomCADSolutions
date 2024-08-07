import HeaderBtn from './header-btn'
import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function LoginBtn() {
    const { t } = useTranslation();

    return (
        <Link to="/login">
            <HeaderBtn icon="user-secret" iconOrder="2" text={t("header.Login")} />
        </Link>
    )
}

export default LoginBtn;