import { useLoaderData, Link } from 'react-router-dom';
import ErrorPage from '@/components/error-page';

function RoleInfo() {
    const { role } = useLoaderData();

    let abilities;
    let guide;
    switch (role) {
        case 'Client':
            abilities = ['Purchase any 3D Model from our Gallery', 'Order a Custom 3D Model', 'Track, Modify and Delete your Orders'];
            guide = ['Visit THIS page: ', 'Provide the necessary info', 'Enjoy the experience!'];
            break;

        case 'Contributor':
            abilities = ['Upload your 3D Models to our Gallery', 'Get 85% of the profit earned', 'Track, Modify and Delete your 3D Models'];
            guide = ['Visit THIS page: ', 'Provide the necessary info', 'Enjoy the experience!'];
            break;

        case 'Designer':
            abilities = ['Become a part of our lovely team of 3D Designers', 'Validate Contributor\'s 3D Models', 'Take on and Complete Clients\' Orders', 'Upload your own 3D Models directly to our Gallery', 'Get 100% of the profit earned + your monthly salary'];
            guide = ['Email us HERE: ', 'Express your desire to join our team', 'Provide relevant experience', 'Make it through an interview', 'Enjoy the experience!'];
            break;

        default: return <ErrorPage status={404} />;
    }

    return (
        <div className="flex flex-col gap-y-8">
            <h1 className="text-3xl text-indigo-900 text-center font-bold">
                <span>Learn about being a </span>
                <span className="text-indigo-800 font-extrabold">{role}</span>!
                <span hidden={role !== 'Designer'}>
                    <Link to="/about" className="block text-sm">
                        (click here if you're interested in a software development position)
                    </Link>
                </span>
            </h1>
            <div className="text-indigo-900">
                <ol className="list-decimal text-lg flex justify-evenly">
                    <li className="text-2xl">
                        <p className="font-bold">What can I do?</p>
                        <span>
                            <p className="text-xl italic">When you're a {role}, you can:</p>
                            <ul className="text-lg ps-8 list-disc">
                                {abilities.map((ability, i) => <li key={i}>{ability}</li>)}
                            </ul>
                        </span>
                    </li>
                    <li className="text-2xl">
                        <p className="font-bold">How do I become one?</p>
                        <span>
                            <p className="text-xl italic">To become a {role}, you must:</p>
                            <ol className="text-lg ps-8 list-decimal">
                                <li>                                    
                                    <Link to={role === 'Designer' ? 'mailto:customcadsolutions222@gmail.com' : `/register/${role}`}>{guide[0]}</Link>
                                </li>
                                {guide.filter((_, i) => i !== 0).map((step, i) => <li key={i}>{step}</li>)}
                            </ol>
                        </span>
                    </li>
                </ol>
            </div>
        </div>
    );
}

export default RoleInfo;