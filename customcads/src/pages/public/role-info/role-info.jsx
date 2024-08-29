import { useLoaderData, Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import ErrorPage from '@/components/error-page';

function RoleInfo() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const { role } = useLoaderData();

    let abilities;
    let guide;
    switch (role) {
        case 'Client':
            abilities = [tPages('role.client_ability_1'), tPages('role.client_ability_2'), tPages('role.client_ability_3')];
            guide = [tPages('role.guide_1'), tPages('role.guide_2'), tPages('role.enjoy')];
            break;

        case 'Contributor':
            abilities = [tPages('role.contributor_ability_1'), tPages('role.contributor_ability_2'), tPages('role.contributor_ability_3')];
            guide = [tPages('role.guide_1'), tPages('role.guide_2'), tPages('role.enjoy')];
            break;

        case 'Designer':
            abilities = [tPages('role.designer_ability_1'), tPages('role.designer_ability_2'), tPages('role.designer_ability_3'), tPages('role.designer_ability_4'), tPages('role.designer_ability_5')];
            guide = [tPages('role.designer_guide_1'), tPages('role.designer_guide_2'), tPages('role.designer_guide_3'), tPages('role.designer_guide_4'), tPages('role.enjoy')];
            break;

        default: return <ErrorPage status={404} />;
    }

    return (
        <div className="mt-5 flex flex-col gap-y-10">
            <h1 className="text-3xl text-indigo-900 text-center font-bold">
                <span>{tPages('role.title')} </span>
                <span className="text-indigo-800 font-extrabold">
                    {tCommon(`roles.${role}`)}
                </span>!
                <span hidden={role !== 'Designer'}>
                    <Link to="/about" className="block text-sm">
                        {tPages('role.software')}
                    </Link>
                </span>
            </h1>
            <div className="text-indigo-900">
                <ol className="list-decimal text-lg flex justify-evenly">
                    <li className="text-2xl list-inside px-8 py-8 rounded-lg bg-indigo-100 border-4 border-indigo-800 shadow-lg shadow-indigo-400">
                        <span className="font-bold">{tPages('role.subtitle_1')}?</span>
                        <span className="mt-4 flex flex-col gap-y-2">
                            <p className="text-xl italic">
                                {tPages('role.abilities', { role: tCommon(`roles.${role}`) })}:
                            </p>
                            <ul className="text-lg ps-8 list-disc">
                                {abilities.map((ability, i) => <li key={i}>{ability}</li>)}
                            </ul>
                        </span>
                    </li>
                    <li className="text-2xl list-inside px-8 py-8 rounded-lg bg-indigo-100 border-4 border-indigo-800 shadow-lg shadow-indigo-400">
                        <span className="font-bold">{tPages('role.subtitle_2')}?</span>
                        <span className="mt-4 flex flex-col gap-y-2">
                            <p className="text-xl italic">
                                {tPages('role.guides', { role: tCommon(`roles.${role}`) })}:
                            </p>
                            <ol className="text-lg ps-8 list-decimal">
                                <li>
                                    <Link to={role === 'Designer' ? 'mailto:customcadsolutions222@gmail.com' : `/register/${role}`}>{guide[0]}</Link>
                                </li>
                                {guide.filter((_, i) => i).map((step, i) => <li key={i}>{step}</li>)}
                            </ol>
                        </span>
                    </li>
                </ol>
            </div>
        </div>
    );
}

export default RoleInfo;