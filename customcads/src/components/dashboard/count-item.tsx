interface CountItemProps {
    text: string
}

function CountItem({ text }: CountItemProps) {
    return (
        <li className="grow flex items-center justify-center border-b-2 border-indigo-400 hover:italic hover:text-2xl hover:font-bold">
            {text}
        </li>
    );
}

export default CountItem;