function Step({ parent, children }) {
    return (
        <div className="w-5/12 flex flex-col justify-center text-center">
            <div className="py-3 bg-indigo-200 border border-indigo-600 mb-2 rounded-md">
                {parent}
            </div>
            <div className="flex justify-center mb-5">
                <div className="absolute border-x-8 border-x-transparent border-t-8 border-t-indigo-500"></div>
            </div>
            <ul className="w-full flex gap-1 p-1 border-2 border-indigo-600 rounded-md">
                {
                    children.map((item, index) =>
                        <li key={index} className="w-1/2 py-3 px-2 bg-indigo-200 border border-indigo-600 rounded-lg">
                            {item}
                        </li>)
                }
            </ul>
        </div>
    );
}

export default Step;