import { Link } from 'react-router-dom'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function Step({ parent, children }) {
    return (
        <div className="w-5/12 flex flex-wrap justify-center text-center">
            <Link to={parent.path} className="basis-full py-3 bg-indigo-200 border border-indigo-600 rounded-md">{parent.content}</Link>
            <div className="basis-full flex justify-center mb-2">
                <FontAwesomeIcon icon="caret-down" className="text-indigo-500 text-2xl" />
            </div>
            <ul className="basis-8/12 flex gap-1 p-1 border-2 border-indigo-600 rounded-md">
                {
                    children.map(item =>
                        <li key={item.id} className="w-1/2 ">
                            <Link to={item.path} className="w-full inline-block py-3 bg-indigo-200 border border-indigo-600 rounded-lg">{item.content}</Link>
                        </li>)
                }
            </ul>
        </div>
    );
}

export default Step;