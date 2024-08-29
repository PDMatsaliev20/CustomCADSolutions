import { useTranslation } from 'react-i18next';
import useErrors from '@/hooks/useErrors';
import userValidation from '@/constants/data/user';

export default (user) => {
    const { t: tCommon } = useTranslation('common');
    const errorMessages = useErrors();
    let errors = {};

    const username = user.username.trim();
    const usernameLabel = tCommon('labels.username');
    const { isRequired: usernameIsRequired, minLength: usernameMinLength, maxLength: usernameMaxLength } = userValidation.username;

    if (usernameIsRequired && !username) {
        errors.username = errorMessages.required(usernameLabel);
    } else if (!(username.length >= usernameMinLength && username.length <= usernameMaxLength)) {
        errors.username = errorMessages.length(usernameLabel, usernameMinLength, usernameMaxLength);
    }

    const password = user.password.trim();
    const passwordLabel = tCommon('labels.password');
    const { isRequired: passwordIsRequired, minLength: passwordMinLength, maxLength: passwordMaxLength } = userValidation.password;

    if (passwordIsRequired && !password) {
        errors.password = errorMessages.required(passwordLabel);
    } else if (!(password.length >= passwordMinLength && password.length <= passwordMaxLength)) {
        errors.password = errorMessages.length(passwordLabel, passwordMinLength, passwordMaxLength);
    }

    return errors;
};;