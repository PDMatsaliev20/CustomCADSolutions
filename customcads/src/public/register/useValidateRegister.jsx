import userValidation from '@/constants/data/user'
import useErrors from '@/hooks/useErrors'
import { useTranslation } from 'react-i18next'

export default (user) => {
    const { t } = useTranslation();
    const errorMessages = useErrors();
    let errors = {};

    const username = user.username.trim();
    const usernameLabel = t('common.labels.Username');
    const { isRequired: usernameIsRequired, minLength: usernameMinLength, maxLength: usernameMaxLength } = userValidation.username;

    if (usernameIsRequired && !username) {
        errors.username = errorMessages.required(usernameLabel);
    } else if (!(username.length >= usernameMinLength && username.length <= usernameMaxLength)) {
        errors.username = errorMessages.length(usernameLabel, usernameMinLength, usernameMaxLength);
    }

    const email = user.email.trim();
    const emailLabel = t('common.labels.Email');
    const { isRequired: emailIsRequired, minLength: emailMinLength, maxLength: emailMaxLength, regex: emailRegex } = userValidation.email;

    if (emailIsRequired && !email) {
        errors.email = errorMessages.required(emailLabel);
    } else if (email.length > emailMaxLength) {
        errors.email = errorMessages.length(emailLabel, emailMinLength, emailMaxLength);
    } else if (!email.includes('@')) {
        errors.email = errorMessages.requiredSymbol(emailLabel, '@');
    } else if (email.startsWith('@')) {
        errors.email = errorMessages.invalidFirstChar('@');
    } else if (email.endsWith('@')) {
        errors.email = errorMessages.invalidLastChar('@');
    } else if (!emailRegex.test(email)) {
        errors.email = errorMessages.invalidPattern(emailLabel);
    }

    const password = user.password.trim();
    const passwordLabel = t('common.labels.Password');
    const { isRequired: passwordIsRequired, minLength: passwordMinLength, maxLength: passwordMaxLength } = userValidation.password;

    if (passwordIsRequired && !password) {
        errors.password = errorMessages.required(passwordLabel);
    } else if (!(password.length >= passwordMinLength && password.length <= passwordMaxLength)) {
        errors.password = errorMessages.length(passwordLabel, passwordMinLength, passwordMaxLength);
    }

    const confirmPassword = user.confirmPassword.trim();
    const confirmPasswordLabel = t('common.labels.Confirm Password');
    const { isRequired: confirmPasswordIsRequired } = userValidation.confirmPassword;

    if (confirmPasswordIsRequired && !confirmPassword) {
        errors.confirmPassword = errorMessages.required(confirmPasswordLabel);
    } else if (confirmPassword !== user.password.trim()) {
        errors.confirmPassword = t('common.errors.Passwords must be equal!');
    }

    return errors;
};