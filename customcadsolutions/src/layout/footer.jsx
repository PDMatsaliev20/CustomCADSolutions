import { Link } from 'react-router-dom'

function Footer() {
    return (
        <footer className="absolute bottom-0 w-full h-24 border-t-2 border-gray-400 rounded-t-lg bg-indigo-100 ">
            <div className="mt-6 flex justify-evenly items-center text-sm">
                <div className="underline font-semibold">
                    <p className="mb-2 text-center"><Link to="/policy">Privacy Policy</Link></p>
                    <p className="mt-2 text-center"><Link to="/about">About Us</Link></p>
                </div>
                <section>
                    <p className="text-lg font-black">
                        &copy; 2023-{new Date().getFullYear()} -
                        <Link to="/"> CustomCADSolutions</Link>
                    </p>
                </section>
                <p className="italic">
                    <span>Contacts:</span>
                    <section>
                        <a href="https://www.instagram.com/customcadsolutions/">
                            <i className="ms-1 fa fa-instagram"></i>
                        </a>
                        <a href="https://twitter.com/customcads/">
                            <i className="ms-1 fa fa-twitter"></i>
                        </a>
                        <a href="#"> {/* Create a Facebook profile */}
                            <i className="ms-1 fa fa-facebook"></i>
                        </a>
                        <a href="https://github.com/NinjataWRLD/CustomCADSolutions/">
                            <i className="ms-1 fa fa-github"></i>
                        </a>
                    </section>
                </p>
            </div>
        </footer>
    );
}

export default Footer;