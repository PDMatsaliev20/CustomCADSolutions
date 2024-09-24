import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';

interface SocialMediaIconProps {
    link: string
    icon: IconProp
}

function SocialMediaIcon({ link, icon }: SocialMediaIconProps) {
    return (
        <Link to={link}>
            <FontAwesomeIcon icon={icon} className="hover:text-indigo-500" />
        </Link>
    );
}

export default SocialMediaIcon;