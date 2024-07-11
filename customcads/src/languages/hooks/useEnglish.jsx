import rolesEn from '@/languages/resources/en/common/roles.json'
import categoriesEn from '@/languages/resources/en/common/categories.json'
import sortingsEn from '@/languages/resources/en/common/sortings.json'
import othersEn from '@/languages/resources/en/common/others.json'

import headerEn from '@/languages/resources/en/header.json'
import navbarEn from '@/languages/resources/en/navbar.json'
import footerEn from '@/languages/resources/en/footer.json'

import homeEn from '@/languages/resources/en/pages/home.json'
import galleryEn from '@/languages/resources/en/pages/gallery.json'
import aboutEn from '@/languages/resources/en/pages/about.json'
import chooseRoleEn from '@/languages/resources/en/pages/choose-role.json'
import registerEn from '@/languages/resources/en/pages/register.json'
import loginEn from '@/languages/resources/en/pages/login.json'

export default () => {
    return {
        translation: {
            header: headerEn,
            navbar: navbarEn,
            body: {
                home: homeEn,
                gallery: galleryEn,
                about: aboutEn,
                chooseRole: chooseRoleEn,
                register: registerEn,
                login: loginEn,
            },
            footer: footerEn,
            common: {
                roles: rolesEn,
                categories: categoriesEn,
                sortings: sortingsEn,
                others: othersEn,
            }
        }
    };
};