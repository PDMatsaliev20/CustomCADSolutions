import { Link } from 'react-router-dom'
import Path from '../components/path'
import BtnLink from '../components/btnlink'
import Profile from '../components/profile'

function HomePage() {
    return (
        <>
            <section className="h-96 flex justify-evenly mt-3 mb-5">
                <article className="flex flex-col justify-evenly text-center">
                    <h2 className="text-5xl font-bold italic">The Land of 3D Models</h2>
                    <div className="flex flex-col gap-3">
                        <span className="text-2xl ">We offer high-quality 3D Models tailored to your needs!</span>
                        <span className="text-lg italic">(*optional 5-10 business day delivery)</span>
                    </div>
                    <div className="mt-4 flex justify-center gap-8">
                        <BtnLink to="/register/client" text="Looking to buy" />
                        <BtnLink to="/register/contributor" text="Looking to sell" />
                    </div>
                </article>
                <aside className="h-full flex items-center">
                    <iframe className="h-full w-96" mozallowfullscreen="true" webkitallowfullscreen="true" allow="autoplay; fullscreen; xr-spatial-tracking" src="https://sketchfab.com/models/99bfe75ebd734fa3832a63e02e2cacf7/embed?autospin=1&autostart=1&transparent=1&ui_animations=0&ui_infos=0&ui_stop=0&ui_inspector=0&ui_watermark_link=0&ui_watermark=0&ui_ar=0&ui_help=0&ui_settings=0&ui_vr=0&ui_fullscreen=0&ui_annotations=0&ui_color=93c5fd"> </iframe>
                </aside>
            </section>
            <hr className="h-px border-0 bg-black" />

            { /* Weird bug where <hr /> alternates between thick and thin? */}
            { /* Temporarily fixed it by adding an invisible <hr /> between them. */}
            <hr className="h-px border-0 bg-transparent" />

            <h3 className="text-4xl text-center mt-5 font-semibold">Two ways to go about this:</h3>
            <section className="my-10">
                <article className="flex justify-evenly items-center gap-5">
                    <Path parent={<Link to="/register/client">Register as Customer</Link>}
                        children={[
                            <Link to="/orders/gallery">Order from our Gallery</Link>,
                            <Link to="/orders/custom">Order from our 3D Designers</Link>,
                        ]} />
                    or
                    <Path parent={<Link to="/register/contributor">Register as Contributor</Link>}
                        children={[
                            <Link to="cads/upload">Upload to Gallery</Link>,
                            <Link to="cads/sell">Sell Directly to Us</Link>
                        ]} />
                </article>
            </section>
            <hr className="h-px border-0 bg-black" />

            <h4 className="my-5 text-3xl text-center font-semibold">Our Team:</h4>
            <section className="mb-5 gap-2 px-5 pt-5 bg-indigo-800 rounded-md">
                <article className="flex">
                    <Profile image="./src/assets/engineer.jpg"
                        name="Ivcho Angela" role="Co-founder, Web Developer"
                        desc="With multiple years of expirience at the humble age of 17,
                        this young man single-handedly designed, built and manages everything about this site,
                        from the database to the back-end and front-end,
                        and soon the deployment of the website to the world-wide web."
                    />
                    <Profile image="./src/assets/designer.jpg"
                        name="Borko Vence Venc" role="Co-founder, 3D Designer"
                        desc="A natural at coming up with and crafting 3D Models that comply with international standards,
                        this handsome adult is solely responsible for filling up our entire storage with 3D Models,
                        be it by answering the custom orders or just publishing his own projects."
                    />
                </article>
                <div className="flex flex-col py-3 items-center text-indigo-50">
                    <span className="">
                        Want to join our team?
                    </span>
                    <span>
                        We're looking for front-end developers and 3d designers!
                    </span>
                    <a href="mailto:customcadsolutions@gmail.com" className="text-sky-300"> Email us HERE</a>
                </div>
            </section>
        </>
    );
}

export default HomePage;