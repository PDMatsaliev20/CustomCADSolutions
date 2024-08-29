import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import HeaderBtn from './header-btn';

function LoginBtn() {
    const { t: tLayout } = useTranslation('layout');

    return (
        <Link to="/login">
            <HeaderBtn icon="user-secret" iconOrder="2" text={tLayout("header.login")} />
        </Link>
    )
}

export default LoginBtn;