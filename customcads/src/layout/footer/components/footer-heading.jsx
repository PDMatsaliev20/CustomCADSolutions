import { Link } from 'react-router-dom'

function FooterHeading() {
    return (
        <header className="text-lg font-bold">
            &copy; 2023-{new Date().getFullYear()} -
            <Link to="/about" className="font-black"> CustomCADs</Link>
        </header>
    );
}

export default FooterHeading;