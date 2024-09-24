import { useTranslation } from 'react-i18next';
import Person from '../interfaces/person';

interface ProfileProps {
    person: Person
}

function Profile({ person: { img, name, role, desc, contact } }: ProfileProps) {
    const { t: tPages } = useTranslation('pages');

    return (
        <article className="flex flex-wrap gap-x-3 p-2 w-full bg-indigo-200 border border-indigo-500 rounded-sm">
            <div className="h-40 asis-1/6">
                <img src={img} className="w-auto h-full rounded-xl" />
            </div>
            <div className="basis-5/6 grow flex flex-col justify-between">
                <details open>
                    <summary>
                        <span className="text-xl font-bold">{name}</span>
                        <p className="italic font-semibold">{role}</p>
                    </summary>
                    <p>{desc}</p>
                </details>
                <p className="underline underline-offset-4">
                    {tPages('about.preferred')}
                    <span className="font-bold"> {contact}</span>
                </p>
            </div>
        </article>
    );
}

export default Profile;