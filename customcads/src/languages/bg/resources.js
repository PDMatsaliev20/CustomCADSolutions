// Layout
import header from './resources/layout/header.json';
import navbar from './resources/layout/navbar.json';
import footer from './resources/layout/footer.json';

// Pages
import home from './resources/pages/home.json';
import gallery from './resources/pages/gallery.json';
import about from './resources/pages/about.json';
import register from './resources/pages/register.json';
import login from './resources/pages/login.json';
import role from './resources/pages/role.json';
import orders from './resources/pages/orders.json';
import cads from './resources/pages/cads.json';
import designer from './resources/pages/designer.json';

// Common
import labels from './resources/common/labels.json';
import errors from './resources/common/errors.json';
import searchbar from './resources/common/searchbar.json';
import pagination from './resources/common/pagination.json';
import placeholders from './resources/common/placeholders.json';
import roles from './resources/common/roles.json';
import categories from './resources/common/categories.json';
import sortings from './resources/common/sortings.json';
import http from './resources/common/http.json';
import statuses from './resources/common/statuses.json';
import others from './resources/common/others.json';

export default {
    layout: {
        header,
        navbar,
        footer
    },
    pages: {
        home,
        gallery,
        about,
        register,
        login,
        role,
        orders,
        cads,
        designer,
    },
    common: {
        labels,
        placeholders,
        errors,
        searchbar,
        pagination,
        roles,
        categories,
        sortings,
        http,
        statuses,
        others,
    }
};