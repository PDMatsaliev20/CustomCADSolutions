import { Link } from 'react-router-dom'

function Step({ parent, children }) {
    return (
        <div className="w-5/12 flex flex-col justify-center text-center">
            <div className="py-3 bg-indigo-200 border border-indigo-600 mb-2 rounded-md">
                <Link to={parent.path}>{parent.content}</Link>
            </div>
            <div className="flex justify-center mb-5">
                <div className="absolute border-x-8 border-x-transparent border-t-8 border-t-indigo-500"></div>
            </div>
            <ul className="w-full flex gap-1 p-1 border-2 border-indigo-600 rounded-md">
                {
                    children.map(item =>
                        <li key={item.id} className="w-1/2 py-3 px-2 bg-indigo-200 border border-indigo-600 rounded-lg">
                            <Link to={item.path}>{item.content}</Link>
                        </li>)
                }
            </ul>
        </div>
    );
}

export default Step;