import labelsBg from '@/languages/resources/bg/common/labels.json'
import errorsBg from '@/languages/resources/bg/common/errors.json'
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

import ordersBg from '@/languages/resources/bg/pages/private/client/user-orders.json'
import ordersDetailsBg from '@/languages/resources/bg/pages/private/client/order-details.json'
import customOrderBg from '@/languages/resources/bg/pages/private/client/custom-order.json'

import cadsBg from '@/languages/resources/bg/pages/private/contributor/user-cads.json'
import cadDetailsBg from '@/languages/resources/bg/pages/private/contributor/cad-details.json'
import editCadBg from '@/languages/resources/bg/pages/private/contributor/edit-cad.json'

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
                customOrder: customOrderBg,
                cads: cadsBg,
                cadDetails: cadDetailsBg,
                editCad: editCadBg,
            },
            footer: footerBg,
            common: {
                labels: labelsBg,
                errors: errorsBg,
                roles: rolesBg,
                categories: categoriesBg,
                sortings: sortingsBg,
                statuses: statusesBg,
                others: othersBg,
            }
        }
    };
};