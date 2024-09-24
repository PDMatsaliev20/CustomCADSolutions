import { useState, useEffect } from 'react';
import { dateToMachineReadable } from '@/utils/date-manager';

interface TagProps {
    tag: string
    label: string
    hidden?: boolean
}

function Tag({ tag, label, hidden }: TagProps) {
    const [element, setElement] = useState(<></>);

    useEffect(() => {
        if (!hidden) {
            switch (tag) {
                case 'delivery':
                    setElement(<div className="font-bold italic px-3 py-1 bg-indigo-50 rounded-lg border border-indigo-300 shadow-md shadow-indigo-400">
                        {label}
                    </div>);
                    break;

                case 'category':
                    setElement(<div className="font-bold px-3 py-1 bg-indigo-50 rounded-lg border border-indigo-300 shadow-md shadow-indigo-400">
                        {label}
                    </div>);
                    break;

                case 'date':
                    setElement(<div className="italic px-3 py-1 bg-indigo-50 rounded-lg border border-indigo-300 shadow-md shadow-indigo-400">
                        <time dateTime={dateToMachineReadable(label)}>
                            {label}
                        </time>
                    </div>);
                    break;

                default: setElement(<></>);
            }
        }
    }, [tag, label, hidden]);

    return element;
}

export default Tag;