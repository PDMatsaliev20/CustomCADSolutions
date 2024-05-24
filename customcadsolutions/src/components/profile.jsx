function Profile({ image, name, role, desc }) {
    return (
        <article className="flex p-2 w-full bg-indigo-200 border border-indigo-500 rounded-sm">
            <img src={image} className="w-1/3 h-auto" />
            <details className="ms-3">
                <summary>
                    <span className="text-2xl font-medium">{name}</span>
                    <p className="italic">{role}</p>
                </summary>
                <p>{desc}</p>
            </details>
        </article>
    );
}

export default Profile;