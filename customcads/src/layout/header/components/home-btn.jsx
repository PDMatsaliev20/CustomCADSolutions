import HeaderBtn from './header-btn'
import { useNavigate } from 'react-router-dom'

function HomeBtn() {
    const navigate = useNavigate();

    const handleClick = () => navigate("/home");

    return <HeaderBtn icon="home" padding="p-2" textSize="text-3xl"
        onClick={handleClick} 
    />;
}

export default HomeBtn;