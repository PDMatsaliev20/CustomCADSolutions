import { Link } from 'react-router-dom';

function FooterLink({ to, text }) {
    return (
        <span className="text-sm font-bold underline">
            <Link to={to} className="hover:italic hover:text-indigo-700">{text}</Link>
        </span>
    );
}

export default FooterLink;