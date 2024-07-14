import rolesBg from '@/languages/resources/bg/common/roles.json'
import categoriesBg from '@/languages/resources/bg/common/categories.json'
import sortingsBg from '@/languages/resources/bg/common/sortings.json'
import statusesBg from '@/languages/resources/bg/common/statuses.json'
import othersBg from '@/languages/resources/bg/common/others.json'

import headerBg from '@/languages/resources/bg/header.json'
import navbarBg from '@/languages/resources/bg/navbar.json'
import footerBg from '@/languages/resources/bg/footer.json'

import homeBg from '@/languages/resources/bg/pages/public/home.json'
import galleryBg from '@/languages/resources/bg/pages/public/gallery.json'
import aboutBg from '@/languages/resources/bg/pages/public/about.json'
import chooseRoleBg from '@/languages/resources/bg/pages/public/choose-role.json'
import registerBg from '@/languages/resources/bg/pages/public/register.json'
import loginBg from '@/languages/resources/bg/pages/public/login.json'

import ordersBg from '@/languages/resources/bg/pages/private/orders.json'
import ordersDetailsBg from '@/languages/resources/bg/pages/private/order-details.json'

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
                orders: ordersBg,
                orderDetails: ordersDetailsBg,
            },
            footer: footerBg,
            common: {
                roles: rolesBg,
                categories: categoriesBg,
                sortings: sortingsBg,
                statuses: statusesBg,
                others: othersBg,
            }
        }
    };
};