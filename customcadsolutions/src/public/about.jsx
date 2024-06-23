import Profile from '../components/profile'

function AboutUsPage() {

    const ivcho = {
        img: './src/assets/engineer.jpg',
        name: 'Ivcho Angela',
        role: 'Co-founder, Web Developer',
        desc: `With multiple years of expirience at the humble age of 17,
               this young man single-handedly designed, built and manages everything about this site,
               from the database to the back-end and front-end, and soon the deployment of the website to the world-wide web.`
    };

    const borko = {
        img: './src/assets/designer.jpg',
        name: 'Borko Vence Venc',
        role: 'Co-founder, 3D Designer',
        desc: `A natural at coming up with and crafting 3D Models that comply with international standards,
               this handsome adult is solely responsible for filling up our entire storage with 3D Models,
               be it by answering the custom orders or just publishing his own projects.`
    };

    return (
        <div className="my-10 bg-indigo-300 p-4 rounded-md">
            <h1 className="text-4xl text-center font-semibold py-7">About Us and Our Team:</h1>
            <section className="mb-5 gap-2 px-5 pt-5 bg-indigo-800 rounded-md">
                <div className="flex flex-col gap-3 xl:flex-row">
                    <article>
                        <Profile person={ivcho} />
                    </article>
                    <article>
                        <Profile person={borko} />
                    </article>
                </div>
                <div className="flex flex-col py-3 items-center text-indigo-50">
                    <span className="">
                        Want to join our team?
                    </span>
                    <span>
                        We're looking for (junior) web developers and 3d designers!
                    </span>
                    <a href="mailto:customcadsolutions@gmail.com" className="text-sky-300"> Email us HERE</a>
                </div>
            </section>
        </div>
    );
}

export default AboutUsPage;