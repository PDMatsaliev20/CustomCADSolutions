import { Link } from 'react-router-dom';

function FooterLink({ to, text }) {
    return (
        <span className="text-sm font-semibold underline">
            <Link to={to}>{text}</Link>
        </span>
    );
}

export default FooterLink;