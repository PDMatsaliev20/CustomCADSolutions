import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import HeaderBtn from './header-btn';

function LoginBtn() {
    const { t } = useTranslation();

    return (
        <Link to="/login">
            <HeaderBtn icon="user-secret" iconOrder="2" text={t("header.Login")} />
        </Link>
    )
}

export default LoginBtn;