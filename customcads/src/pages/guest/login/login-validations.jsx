import { useTranslation } from 'react-i18next';
import user from '@/constants/data/user';

export default () => {
    const { t: tCommon } = useTranslation('common');
    
    const { username, password } = user;

    const usernameInfo = { field: tCommon('labels.username'), min: username.minLength, max: username.maxLength };
    const usernameValidation = {
        required: {
            value: username.isRequired,
            message: tCommon('errors.required', usernameInfo),
        },
        minLength: {
            value: username.minLength,
            message: tCommon('errors.length', usernameInfo),
        },
        maxLength: {
            value: username.maxLength,
            message: tCommon('errors.length', usernameInfo),
        },
    };

    const passwordInfo = { field: tCommon('labels.password'), min: password.minLength, max: password.maxLength };
    const passwordValidation = {
        required: {
            value: password.isRequired,
            message: tCommon('errors.required', passwordInfo),
        },
        minLength: {
            value: password.minLength,
            message: tCommon('errors.length', passwordInfo),
        },
        maxLength: {
            value: password.maxLength,
            message: tCommon('errors.length', passwordInfo),
        },
    };

    return {
        username: usernameValidation,
        password: passwordValidation
    };
};