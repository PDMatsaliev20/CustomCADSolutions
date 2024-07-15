import { Disclosure, Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/react'
import { useTranslation } from 'react-i18next'
import { useContext } from 'react'
import AuthContext from '@/components/auth-context'
import HeaderBtn from './header-btn'

export default function Example({ onLogout, username }) {
    const { t } = useTranslation();
    const { userRole } = useContext(AuthContext);

    return (
        <Disclosure as="div" className="relative">
            <Menu as="div" className="relative">
                <MenuButton as="div">
                    <HeaderBtn icon="id-card" textSize="text-3xl" padding="p-2" />
                </MenuButton>
                <MenuItems transition
                    className="absolute right-0 mt-2 z-10 py-2 text-center text-indigo-800 
                                rounded-md bg-indigo-50 border border-indigo-500 w-60 shadow-lg shadow-indigo-400
                                data-[closed]:scale-95 data-[closed]:transform data-[closed]:opacity-0 
                                data-[enter]:duration-150 data-[enter]:ease-out 
                                data-[leave]:duration-75 data-[leave]:ease-in"
                >
                    <div className="flex flex-wrap justify-center gap-y-2">
                        <MenuItem as="div" className="basis-full flex gap-x-2 justify-center items-center">
                            <span className="text-lg font-extrabold">{username}</span>
                        </MenuItem>

                        <MenuItem as="div" className="basis-full">
                            <span className="italic font-bold">
                                Role: {t(`common.roles.${userRole}`)}
                            </span>
                        </MenuItem>
                        
                        <div className="basis-full border-b-2 border-indigo-700 my-2 mb-1"></div>

                        <div>
                            <MenuItem as="div" className="basis-full">
                                <button className="font-bold text-indigo-900 bg-indigo-200 py-2 px-5 rounded border border-indigo-700 shadow shadow-indigo-800" onClick={onLogout}>
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
