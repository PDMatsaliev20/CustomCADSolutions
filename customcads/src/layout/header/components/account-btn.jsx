import { Logout } from '@/requests/public/identity'
import { Disclosure, Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/react'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useContext } from 'react'
import AuthContext from '@/components/auth-context'
import HeaderBtn from './header-btn'

export default function AccountBtn() {
    const navigate = useNavigate();
    const { t } = useTranslation();
    const { userRole, username, setIsAuthenticated } = useContext(AuthContext);

    const handleLogout = async () => {
        try {
            await Logout();
            setIsAuthenticated(false);
            navigate('/');
        } catch (e) {
            alert(e.response.data);
        }
    };

    return (
        <Disclosure as="div" className="relative">
            <Menu as="div" className="relative">
                <MenuButton as="div">
                    <HeaderBtn icon="id-card" iconOrder="2" text={username} />
                </MenuButton>
                <MenuItems transition
                    className="absolute left-0 mt-2 z-10 text-center text-indigo-800 rounded-md bg-indigo-50 border border-indigo-500 w-60 shadow-lg shadow-indigo-400
                                data-[closed]:scale-95 data-[closed]:transform data-[closed]:opacity-0 data-[enter]:duration-150 data-[enter]:ease-out data-[leave]:duration-75 data-[leave]:ease-in"
                >
                    <div className="my-4 flex flex-col gap-y-4">
                        <MenuItem as="div" className="basis-full">
                            <span className="italic font-bold">
                                {`${t('header.Role')} ${t(`common.roles.${userRole}`)}`}
                            </span>
                        </MenuItem>

                        <div className="basis-full border-b-2 border-indigo-700"></div>

                        <div>
                            <MenuItem as="div" className="basis-full">
                                <button className="font-bold text-indigo-900 bg-indigo-200 py-2 px-5 rounded border border-indigo-700 shadow shadow-indigo-800" onClick={handleLogout}>
                                    {t('header.Log out')}
                                </button>
                            </MenuItem>
                        </div>
                    </div>
                </MenuItems>
            </Menu>
        </Disclosure>
    )
}
