import ThreeJS from './three.js';
import ICad from '@/interfaces/cad.js';

interface CadProps {
    cad?: ICad
    isHomeCad?: boolean
}

function Cad({ cad, isHomeCad }: CadProps) {
    return (
        <div className="h-full rounded-3xl overflow-hidden bg-indigo-100 border-2 border-indigo-500 shadow-md shadow-indigo-700 hover:bg-indigo-200 active:bg-indigo-300"> 
            <ThreeJS cad={cad} isHomeCad={isHomeCad} />
        </div>
    );
}

export default Cad;