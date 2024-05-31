function Profile({ person }) {
    const { img, name, role, desc } = person;

    return (
        <article className="flex p-2 w-full bg-indigo-200 border border-indigo-500 rounded-sm">
            <img src={img} className="w-32 h-auto" />
            <details className="ms-3" open>
                <summary>
                    <span className="text-xl font-bold">{name}</span>
                    <p className="italic font-semibold">{role}</p>
                </summary>
                <p>{desc}</p>
            </details>
        </article>
    );
}

export default Profile;