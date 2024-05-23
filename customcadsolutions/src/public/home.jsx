import Path from '../components/step'

function HomePage() {
    return (
        <>
            <section className="h-96 flex justify-evenly mt-3 mb-5">
                <article className="flex flex-col justify-evenly text-center">
                    <p className="text-5xl font-bold italic">The Land of 3D Models</p>
                    <div className="flex flex-col gap-3">
                        <span className="text-2xl ">We offer high-quality 3D Models tailored to your needs!</span>
                        <span className="text-lg italic">(*optional 5-10 business day delivery)</span>
                    </div>
                    <div className="mt-4 flex justify-center gap-8">
                        <button className="w-1/3 bg-indigo-500 rounded p-3">
                            <span className="text-lg text-indigo-50 font-bold">Buying? Here.</span>
                        </button>
                        <button className="w-1/3 bg-indigo-500 rounded p-3">
                            <span className="text-lg text-indigo-50 font-bold">Selling? Here.</span>
                        </button>
                    </div>
                </article>
                <aside className="h-full flex items-center">
                    <iframe className="h-full w-96" mozallowfullscreen="true" webkitallowfullscreen="true" allow="autoplay; fullscreen; xr-spatial-tracking" src="https://sketchfab.com/models/99bfe75ebd734fa3832a63e02e2cacf7/embed?autospin=1&autostart=1&transparent=1&ui_animations=0&ui_infos=0&ui_stop=0&ui_inspector=0&ui_watermark_link=0&ui_watermark=0&ui_ar=0&ui_help=0&ui_settings=0&ui_vr=0&ui_fullscreen=0&ui_annotations=0&ui_color=93c5fd"> </iframe>
                </aside>
            </section>
            <hr className="h-px border-0 bg-black" />

            <section className="my-10 flex justify-evenly items-center gap-5">
                <Path parent={"Register as Customer"}
                    children={["Order from our Gallery",
                        "Order from our 3D Designers"]} />
                or
                <Path parent={"Register as Contributor"}
                    children={["Upload your Models to Gallery",
                        "Sell your Models Directly to Us"]} />
            </section>
        </>
    );
}

export default HomePage;