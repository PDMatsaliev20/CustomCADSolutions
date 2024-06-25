import { Link } from 'react-router-dom'

function Footer() {
    return (
        <footer className="basis-full grow sticky z-50 self-end py-4 border-t-2 border-indigo-300 rounded-t-lg bg-indigo-100 ">
            <div className="flex justify-evenly items-center">
                <section className="flex gap-6 underline-offset-4 underline font-semibold text-sm">
                    <p className="text-center"><Link to="/policy">Privacy Policy</Link></p>
                    <p className="text-center"><Link to="/about">About Us</Link></p>
                </section>
                <section>
                    <p className="text-lg font-bold">
                        &copy; 2023-{new Date().getFullYear()} -
                        <Link to="/" className="font-black"> CustomCADSolutions</Link>
                    </p>
                </section>
                <section className="italic">
                    <span>Contacts:</span>
                    <div className="ms-1 inline">
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
                    </div>
                </section>
            </div>
        </footer>
    );
}

export default Footer;