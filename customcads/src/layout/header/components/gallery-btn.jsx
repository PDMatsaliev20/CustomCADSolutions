import HeaderBtn from './header-btn'
import { useNavigate } from 'react-router-dom'

function GalleryBtn() {
    const navigate = useNavigate();

    const handleClick = () => navigate("/gallery");

    return <HeaderBtn icon="basket-shopping" padding="p-2" textSize="text-3xl"
        onClick={handleClick}
    />;
}

export default GalleryBtn;