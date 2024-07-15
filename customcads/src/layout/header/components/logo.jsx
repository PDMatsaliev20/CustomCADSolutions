import { Link } from 'react-router-dom'

function Logo() {
    return (
        <Link to="/about" className="w-56">
            <img src="/src/assets/logo.png" className="w-full h-auto hover:opacity-60" />
        </Link>
    );
}

export default Logo;