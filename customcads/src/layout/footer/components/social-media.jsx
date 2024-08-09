import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function SocialMediaIcon({ link, icon }) {
    return (
        <Link to={link}>
            <FontAwesomeIcon icon={icon} />
        </Link>
    );
}

export default SocialMediaIcon;