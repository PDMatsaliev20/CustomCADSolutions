import { Link } from 'react-router-dom'

function GuestMenu() {
    return (
        <ul>
            <li className="float-left ms-5">
                <Link to="/login" className="text-lg">Log in</Link>
            </li>
            <li className="float-left ms-5">
                <Link to="/register" className="text-lg">Register</Link>
            </li>
        </ul>
    );
}

export default GuestMenu;