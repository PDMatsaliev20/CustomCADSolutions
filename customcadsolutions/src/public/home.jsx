import Path from '../components/path'
import BtnLink from '../components/btn-link'
import Cad from '../cad'

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
                <aside className="h-96 w-96 flex items-center">    
                    <div id="model-0" className="w-full h-full">
                        <Cad id={0} isHomeCad />
                    </div>
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