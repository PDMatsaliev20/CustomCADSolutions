import rolesBg from '@/languages/resources/bg/common/roles.json'
import categoriesBg from '@/languages/resources/bg/common/categories.json'
import sortingsBg from '@/languages/resources/bg/common/sortings.json'
import othersBg from '@/languages/resources/bg/common/others.json'

import headerBg from '@/languages/resources/bg/header.json'
import navbarBg from '@/languages/resources/bg/navbar.json'
import footerBg from '@/languages/resources/bg/footer.json'

import homeBg from '@/languages/resources/bg/pages/home.json'
import galleryBg from '@/languages/resources/bg/pages/gallery.json'
import aboutBg from '@/languages/resources/bg/pages/about.json'
import chooseRoleBg from '@/languages/resources/bg/pages/choose-role.json'
import registerBg from '@/languages/resources/bg/pages/register.json'
import loginBg from '@/languages/resources/bg/pages/login.json'

export default () => {
    return {
        translation: {
            header: headerBg,
            navbar: navbarBg,
            body: {
                home: homeBg,
                gallery: galleryBg,
                about: aboutBg,
                chooseRole: chooseRoleBg,
                register: registerBg,
                login: loginBg,
            },
            footer: footerBg,
            common: {
                roles: rolesBg,
                categories: categoriesBg,
                sortings: sortingsBg,
                others: othersBg,
            }
        }
    };
};