import rolesEn from '@/languages/resources/en/common/roles.json'
import categoriesEn from '@/languages/resources/en/common/categories.json'
import sortingsEn from '@/languages/resources/en/common/sortings.json'
import statusesEn from '@/languages/resources/en/common/statuses.json'
import othersEn from '@/languages/resources/en/common/others.json'

import headerEn from '@/languages/resources/en/header.json'
import navbarEn from '@/languages/resources/en/navbar.json'
import footerEn from '@/languages/resources/en/footer.json'

import homeEn from '@/languages/resources/en/pages/public/home.json'
import galleryEn from '@/languages/resources/en/pages/public/gallery.json'
import aboutEn from '@/languages/resources/en/pages/public/about.json'
import chooseRoleEn from '@/languages/resources/en/pages/public/choose-role.json'
import registerEn from '@/languages/resources/en/pages/public/register.json'
import loginEn from '@/languages/resources/en/pages/public/login.json'

import ordersBg from '@/languages/resources/en/pages/private/orders.json'
import ordersDetailsBg from '@/languages/resources/en/pages/private/order-details.json'

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
                orders: ordersBg,
                orderDetails: ordersDetailsBg,
            },
            footer: footerEn,
            common: {
                roles: rolesEn,
                categories: categoriesEn,
                sortings: sortingsEn,
                statuses: statusesEn,
                others: othersEn,
            }
        }
    };
};