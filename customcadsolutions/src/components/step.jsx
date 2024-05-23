function Step({ parent, children }) {
    return (
        <div className="w-5/12 flex flex-col justify-center text-center">
            <div className="py-3 bg-indigo-200 border border-black mb-2">
                {parent}
            </div>
            <div className="flex justify-center mb-5">
                <div className="absolute arrow-down"></div>
            </div>
            <ul className="w-full flex gap-1">
                {
                    children.map((item, index) =>
                        <li key={index} className="w-1/2 py-3 px-2 bg-indigo-200 border border-black">
                            {item}
                        </li>)
                }
            </ul>
        </div>
    );
}

export default Step;