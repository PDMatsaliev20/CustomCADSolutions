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
            <section className="flex justify-evenly">
                <article className="flex flex-col justify-evenly text-center">
                    <h1 className="text-5xl font-bold italic">The Land of 3D Models</h1>
                    <div className="flex flex-col gap-3">
                        <span className="text-2xl ">We offer high-quality 3D Models tailored to your needs!</span>
                        <span className="text-lg italic">(*optional 5-10 business day delivery)</span>
                    </div>
                    <div className="mt-4 flex justify-center gap-8">
                        <BtnLink to="/register/client" text="Looking to buy" />
                        <BtnLink to="/register/contributor" text="Looking to sell" />
                    </div>
                </article>
                <aside className="h-96 basis-2/5 flex items-center">
                    <div id="model-253" className="w-full h-full">
                        <Cad isHomeCad />
                    </div>
                </aside>
            </section>

            <hr className="border-t border-black" />

            <h3 className="text-4xl text-center mt-8 font-semibold">Two ways to go about this:</h3>
            <section className="my-10">
                <article className="flex justify-evenly items-center gap-5">
                    <Path parent={customerParent} children={customerChildren} />
                    <div className="border-x-2 border-indigo-700 w-3 h-32"></div>
                    <Path parent={contributorParent} children={contributorChildren} />
                </article>
            </section>
        </>
    );
}

export default HomePage;