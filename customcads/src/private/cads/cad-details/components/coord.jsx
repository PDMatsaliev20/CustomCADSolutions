import { useState, useEffect, useRef } from 'react'

function Coord({ type, name, value, relatedValue, setValue, setRelatedValue }) {
    const intervalRef = useRef(null);
    const timeoutRef = useRef(null);

    const [intervalTime, setIntervalTime] = useState(50);

    const handlePlusClick = () => {
        value++;
        setValue(value);
        if (type === 'pan') {
            relatedValue++;
            setRelatedValue(relatedValue);
        }
    };
    const handleMinusClick = () => {
        value--;
        setValue(value);
        if (type === 'pan') {
            relatedValue--;
            setRelatedValue(relatedValue);
        }
    };

    const clearTimers = () => {
        if (intervalRef.current) {
            clearInterval(intervalRef.current);
            intervalRef.current = null;
        }
        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
            timeoutRef.current = null;
        }
    };

    useEffect(() => {
        return () => {
            clearTimers();
        };
    }, []);

    const handleClick = (operation) => {
        if (operation === '+') {
            window.dispatchEvent(new CustomEvent('onCoordChange', {
                detail: {
                    coordType: type,
                    coordName: name,
                    coordValue: value + 1,
                }
            }));
            handlePlusClick();
        } else if (operation === '-') {
            window.dispatchEvent(new CustomEvent('onCoordChange', {
                detail: {
                    coordType: type,
                    coordName: name,
                    coordValue: value - 1,
                }
            }));
            handleMinusClick();
        }
    };

    const handleMouseDown = (handleChange) => {
        handleChange();

        timeoutRef.current = setTimeout(() => {
            intervalRef.current = setInterval(() => {
                handleChange();
            }, intervalTime);
        }, 350);;

        document.addEventListener('mouseup', handleMouseUp);
        document.addEventListener('touchend', handleMouseUp);
    };

    const handleMouseUp = () => {
        clearTimers();
        document.removeEventListener('mouseup', handleMouseUp);
        document.removeEventListener('touchend', handleMouseUp);
    };

    return (
        <div className="basis-full flex flex-wrap justify-center gap-y-2">
            <div className="flex justify-center items-center basis-full gap-x-3">
                <button
                    onMouseDown={() => handleMouseDown(() => handleClick('-'))}
                    onTouchStart={() => handleMouseDown(() => handleClick('-'))}
                    onMouseUp={handleMouseUp}
                    onTouchEnd={handleMouseUp}
                    className="bg-indigo-400 w-8 h-8 rounded-md border border-indigo-600"
                >
                    -
                </button>
                <span>{name}</span>
                <button
                    onMouseDown={() => handleMouseDown(() => handleClick('+'))}
                    onTouchStart={() => handleMouseDown(() => handleClick('+'))}
                    onMouseUp={handleMouseUp}
                    onTouchEnd={handleMouseUp}
                    className="bg-indigo-400 w-8 h-8 rounded-md border border-indigo-600"
                >
                    +
                </button>
            </div>
            <p className="basis-1/3 bg-indigo-200 rounded text-center border-2 border-indigo-300">{value}</p>
        </div>
    );
}

export default Coord;