import HomeBtn from './components/home-btn'
import SearchBar from './components/search-bar'
import GalleryBtn from './components/gallery-btn'
import UserBtn from './components/user-btn'
import LanguageBtn from './components/language-btn'
import Logo from './components/logo'

function Header() {
    return (
        <header className="bg-indigo-200 py-4 border-b border-indigo-700 py-1">
            <ul className="flex mx-5 justify-between items-center">
                <li className="flex gap-x-6 items-center">
                    <HomeBtn />
                    <Logo />
                </li>
                <li className="basis-5/12 flex gap-x-4 items-center">
                    <SearchBar />
                </li>
                <li className="flex gap-x-4">
                    <GalleryBtn />
                    <UserBtn />
                    <LanguageBtn />
                </li>
            </ul>
        </header>
    );
}

export default Header;