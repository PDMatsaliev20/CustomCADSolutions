import { Link } from 'react-router-dom'

function Footer() {
    return (
        <footer className="absolute bottom-0 w-full h-16 border-t-2 border-gray-400 rounded-t-lg bg-indigo-100 ">
            <div className="mt-6 flex justify-evenly text-sm">
                <p className="font-bold">
                    <Link to="/policy">Privacy Policy</Link>
                </p>
                <p>
                    &copy; 2023-{new Date().getFullYear()} -
                    <Link to="/"> CustomCADSolutions</Link>
                </p>
                <p className="rounded-2">
                    Contacts:
                    <a href="https://www.instagram.com/customcadsolutions/">
                        <i className="ms-1 fa fa-instagram"></i>
                    </a>
                    <a href="https://twitter.com/customcads/">
                        <i className="ms-1 fa fa-twitter"></i>
                    </a>
                </p>
            </div>
        </footer>
    );
}

export default Footer;