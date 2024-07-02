function Profile({ person }) {
    const { img, name, role, desc } = person;

    return (
        <article className="flex gap-x-3 p-2 w-full bg-indigo-200 border border-indigo-500 rounded-sm">
            <div className="min-w-[40%]">   
                <img src={img} className="w-full h-auto" />
            </div>
            <div>
                <details open>
                    <summary>
                        <span className="text-xl font-bold">{name}</span>
                        <p className="italic font-semibold">{role}</p>
                    </summary>
                    <p>{desc}</p>
                </details>
            </div>
        </article>
    );
}

export default Profile;