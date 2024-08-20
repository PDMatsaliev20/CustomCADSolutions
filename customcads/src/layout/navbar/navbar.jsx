import { useAuth } from '@/contexts/auth-context';
import GuestNavs from './components/guest-navs';
import ClientNavs from './components/client-navs';
import ContributorNavs from './components/contributor-navs';
import DesignerNavs from './components/designer-navs';

function Navbar() {
    const { isAuthenticated, userRole } = useAuth();

    let navs = <GuestNavs />;
    if (isAuthenticated) {
        switch (userRole) {
            case 'Client': navs = <ClientNavs />; break;
            case 'Contributor': navs = <ContributorNavs />; break;
            case 'Designer': navs = <DesignerNavs />; break;
            case 'Administrator': navs = <p>admin navs</p>; break;
        }
    }

    return (
        <nav className="text-indigo-900 font-bold min-h-8">
            {navs}
        </nav>
    );
}

export default Navbar;