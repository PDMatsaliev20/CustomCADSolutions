import LoginBtn from './login-btn'
import AccountBtn from './account-btn'
import AuthContext from '@/components/auth-context'
import { useContext } from 'react'

function UserBtn() {
    const { isAuthenticated } = useContext(AuthContext);
    
    let button = <AccountBtn />;
    if (!isAuthenticated) {
        button = <LoginBtn />;
    }

    return button;
}

export default UserBtn;