import { Link } from 'react-router-dom'

function BtnLink({ to, text }) {
    return (
        <Link to={to} className="bg-indigo-500 rounded-md py-3 px-12">
            <button>
                <span className="text-lg text-indigo-50 font-bold">{text}</span>
            </button>
        </Link>
    );
}

export default BtnLink;