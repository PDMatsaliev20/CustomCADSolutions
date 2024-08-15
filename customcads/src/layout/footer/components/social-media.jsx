import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function SocialMediaIcon({ link, icon }) {
    return (
        <Link to={link}>
            <FontAwesomeIcon icon={icon} className="hover:text-indigo-500" />
        </Link>
    );
}

export default SocialMediaIcon;