import AuthContext from '../auth-context'
import PublicNavs from '../components/public-navs'
import ClientNavs from '../components/client-navs'
import ContributorNavs from '../components/contributor-navs'
import { useContext } from 'react'

function Navbar() {
    const { isAuthenticated, userRole } = useContext(AuthContext);

    return (
        <nav className="bg-indigo-300 rounded-b-lg shadow-md py-3">
            <div className="flex justify-around text-sm">
                <PublicNavs />
                <ClientNavs shouldBlur={!isAuthenticated} shouldHide={isAuthenticated && userRole !== 'Client'} />
                <ContributorNavs shouldBlur={!isAuthenticated} shouldHide={isAuthenticated && userRole !== 'Contributor'} />
            </div>
        </nav>
    );
}

export default Navbar;