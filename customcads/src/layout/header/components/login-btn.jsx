import { useNavigate } from 'react-router-dom'
import HeaderBtn from './header-btn'

function LoginBtn() {
    const navigate = useNavigate();

    return <HeaderBtn icon="user-secret" padding="p-2" textSize="text-3xl"
        onClick={() => navigate('/login')} />;
}

export default LoginBtn;