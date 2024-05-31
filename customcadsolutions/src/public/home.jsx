import Path from '../components/path'
import BtnLink from '../components/btn-link'

function HomePage() {

    const customerParent = { path: '/register/client', content: 'Register as Client' };
    const customerChildren = [
        { id: 1, path: '/orders/gallery', content: "Order from our Gallery" },
        { id: 2, path: '/orders/custom', content: "Order from our 3D Designers" },
    ];

    const contributorParent = { path: '/register/contributor', content: 'Register as Contributor' };
    const contributorChildren = [
        { id: 3, path: 'cads/upload', content: "Upload to Gallery" },
        { id: 4, path: 'cads/sell', content: "Sell Directly to Us" },
    ];

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
                    <Path parent={customerParent} children={customerChildren} />
                    or
                    <Path parent={contributorParent} children={contributorChildren} />
                </article>
            </section>
        </>
    );
}

export default HomePage;