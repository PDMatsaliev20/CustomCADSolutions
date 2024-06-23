import { Link } from 'react-router-dom'

function PublicNavigationalMenu() {
    return (
        <ul>
            <li className="float-left ms-2 me-4">
                <Link to="/home">Home</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/gallery">Gallery</Link>
            </li>
        </ul>
    );
}

export default PublicNavigationalMenu;