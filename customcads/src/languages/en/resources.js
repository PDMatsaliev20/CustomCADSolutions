// Layout
import header from './resources/header.json';
import navbar from './resources/navbar.json';
import footer from './resources/footer.json';

// Public
import home from './resources/pages/public/home.json';
import gallery from './resources/pages/public/gallery.json';
import about from './resources/pages/public/about.json';
import register from './resources/pages/public/register.json';
import login from './resources/pages/public/login.json';

// Private
import orders from './resources/pages/private/orders.json';
import cads from './resources/pages/private/cads.json';
import designer from './resources/pages/private/designer.json';

// Common
import labels from './resources/common/labels.json';
import placeholders from './resources/common/placeholders.json';
import errors from './resources/common/errors.json';
import searchbar from './resources/common/searchbar.json';
import roles from './resources/common/roles.json';
import categories from './resources/common/categories.json';
import sortings from './resources/common/sortings.json';
import statuses from './resources/common/statuses.json';
import others from './resources/common/others.json';

export default {
    translation: {
        header, navbar, footer,
        public: {
            home,
            gallery,
            about,
            register,
            login,
        },
        private: {
            orders,
            cads,
            designer,
        },
        common: {
            labels,
            placeholders,
            errors,
            searchbar,
            roles,
            categories,
            sortings,
            statuses,
            others,
        }
    }
};