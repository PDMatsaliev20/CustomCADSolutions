import labelsEn from '@/languages/resources/en/common/labels.json'
import errorsEn from '@/languages/resources/en/common/errors.json'
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

import ordersEn from '@/languages/resources/en/pages/private/client/user-orders.json'
import ordersDetailsEn from '@/languages/resources/en/pages/private/client/order-details.json'
import customOrderEn from '@/languages/resources/en/pages/private/client/custom-order.json'

import cadsEn from '@/languages/resources/en/pages/private/contributor/user-cads.json'
import cadDetailsEn from '@/languages/resources/en/pages/private/contributor/cad-details.json'
import editCadEn from '@/languages/resources/en/pages/private/contributor/edit-cad.json'
import uploadCadEn from '@/languages/resources/en/pages/private/contributor/upload-cad.json'

import unvalidatedCadsEn from '@/languages/resources/en/pages/private/designer/contributor-cads.json'

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
                orders: ordersEn,
                orderDetails: ordersDetailsEn,
                customOrder: customOrderEn,
                cads: cadsEn,
                cadDetails: cadDetailsEn,
                editCad: editCadEn,
                uploadCad: uploadCadEn,
                unvalidatedCads: unvalidatedCadsEn,
            },
            footer: footerEn,
            common: {
                labels: labelsEn,
                errors: errorsEn,
                roles: rolesEn,
                categories: categoriesEn,
                sortings: sortingsEn,
                statuses: statusesEn,
                others: othersEn,
            }
        }
    };
};