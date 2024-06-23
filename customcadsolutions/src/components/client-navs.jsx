import { Link } from 'react-router-dom'

function ClientNavigationalMenu({ shouldBlur, shouldHide }) {

    const handleClick = () => {
        if (shouldBlur) {
            alert('Only for Clients!');
        }
    }

    return (
        <ul className={`${shouldHide ? "hidden" : ''} ${shouldBlur ? "blur-sm" : ''}`} onClick={handleClick}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>Your Orders</span>
                        </li>
                        <li className="float-left me-4">
                            <span>Order Custom 3D Model</span>
                        </li>
                        <li className="float-left me-4">
                            <span>Order from Gallery</span>
                        </li>
                    </>
                    :
                    <>
                        <li className="float-left me-4">
                            <Link to="/orders">Your Orders</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/custom">Order Custom 3D Model</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/gallery">Order from Gallery</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ClientNavigationalMenu;