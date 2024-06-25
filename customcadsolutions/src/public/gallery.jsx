import axios from 'axios'
import { useState, useEffect } from 'react'
import Cad from '../cad'

function GalleryPage() {
    const [cads, setCads] = useState([]);
    const [totalCount, setTotalCount] = useState(0);

    useEffect(() => {
        if (!cads.length) {
            getCads();
        }
    }, [cads]);

    return (
        <>
            <h1 className="mt-3 text-3xl text-center font-bold">Our Gallery</h1>
            <section className="my-10">
                <ul className="flex flex-wrap justify-between gap-y-12 gap-x-3">
                    {cads.map(cad =>
                        <>
                            <li key={cad.id} className="basis-3/12 shrink">
                                <div id={`model-${cad.id}`} className="aspect-square bg-indigo-100 w-full"></div>
                            </li>
                            <li key={cad.id} className="basis-3/12 shrink">
                                <div id={`model-${cad.id}`} className="aspect-square bg-indigo-100 w-full"></div>
                            </li>
                            <li key={cad.id} className="basis-3/12 shrink">
                                <div id={`model-${cad.id}`} className="aspect-square bg-indigo-100 w-full"></div>
                            </li>
                            <li key={cad.id} className="basis-3/12 shrink">
                                <div id={`model-${cad.id}`} className="aspect-square bg-indigo-100 w-full"></div>
                            </li>


                        </>
                    )}
                </ul>
            </section>
        </>
    )
    ;

    async function getCads() {
        const { cads, count } = await axios.get('https://localhost:7127/API/Home/Gallery')
            .then(response => response.data);

        setCads([...cads]);
        setTotalCount(count);
    }

    return <>a</>;
}

export default GalleryPage;