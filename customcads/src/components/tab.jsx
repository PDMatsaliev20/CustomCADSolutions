import { Link } from 'react-router-dom';

function Tab({ to, text, isActive, position }) {

    return (
        <Link to={to}
            className={`basis-1/3 bg-indigo-300 py-4 hover:no-underline ${!isActive ? 'hover:opacity-80 ' : ''} active:bg-indigo-400 ${position === 'middle' ? 'border-x-2 border-indigo-700' : ''} ${isActive ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
        >
            {text}
        </Link>
    );
}

export default Tab;