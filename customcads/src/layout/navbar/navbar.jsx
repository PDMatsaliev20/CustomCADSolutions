import AuthContext from '@/components/auth-context'
import ClientNavs from './components/client-navs'
import ContributorNavs from './components/contributor-navs'
import DesignerNavs from './components/designer-navs'
import { useContext } from 'react'

function Navbar() {
    const { isAuthenticated, userRole } = useContext(AuthContext);

    let navs;
    if (isAuthenticated) {
        switch (userRole) {
            case 'Client': navs = <ClientNavs />; break;
            case 'Contributor': navs = <ContributorNavs />; break;
            case 'Designer': navs = <DesignerNavs />; break;
            case 'Administrator': navs = <p>admin navs</p>; break;
        }
    }

    return (
        <nav className="text-indigo-900 font-bold bg-indigo-300 rounded-b-lg px-5 py-3 shadow-lg shadow-indigo-400 min-h-8">
            {navs}
        </nav>
    );
}

export default Navbar;