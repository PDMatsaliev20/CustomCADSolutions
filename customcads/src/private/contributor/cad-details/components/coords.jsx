import Coord from './coord'

function Coords({ type,  title, coords, relatedCoords, setCoords, setRelatedCoords }) {

    return (
        <div className="flex flex-wrap gap-y-4 text-lg items-center">
            <h3 className="basis-full text-xl text-indigo-800 text-center font-semibold">{title}</h3>
            <div className="basis-full flex flex-wrap justify-evenly gap-y-8">
                <Coord type={type} name="X" value={coords[0]} relatedValue={relatedCoords[0]} setValue={(coord) => setCoords([coord, coords[1], coords[2]])} setRelatedValue={(coord) => setRelatedCoords([coord, relatedCoords[1], relatedCoords[2]])} />
                <Coord type={type} name="Y" value={coords[1]} relatedValue={relatedCoords[1]} setValue={(coord) => setCoords([coords[0], coord, coords[2]])} setRelatedValue={(coord) => setRelatedCoords([relatedCoords[0], coord, relatedCoords[2]])} />
                <Coord type={type} name="Z" value={coords[2]} relatedValue={relatedCoords[2]} setValue={(coord) => setCoords([coords[0], coords[1], coord])} setRelatedValue={(coord) => setRelatedCoords([relatedCoords[0], relatedCoords[1], coord])} />
            </div>
        </div>
    );
}

export default Coords;