import PublicNavs from '../components/public-navs'
import ClientNavs from '../components/client-navs'
import ContributorNavs from '../components/contributor-navs'

function Navbar({ isAuthenticated, userRole }) {
    return (
        <nav className="sticky top-0 z-50 bg-indigo-300 rounded-b-lg shadow-md py-3">
            <div className="flex justify-around text-sm">
                <PublicNavs />
                <ClientNavs shouldBlur={!isAuthenticated} shouldHide={isAuthenticated && userRole !== 'Client'} />
                <ContributorNavs shouldBlur={!isAuthenticated} shouldHide={isAuthenticated && userRole !== 'Contributor'} />
            </div>
        </nav>
    );
}

export default Navbar;