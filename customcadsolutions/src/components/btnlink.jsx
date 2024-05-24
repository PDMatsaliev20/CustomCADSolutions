import { Link } from 'react-router-dom'

function BtnLink({ to, text }) {
    return (
        <Link to={to} className="w-1/3 bg-indigo-500 rounded-md p-3">
            <button>
                <span className="text-lg text-indigo-50 font-bold">{text}</span>
            </button>
        </Link>
    );
}

export default BtnLink;