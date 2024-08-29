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

    const firstName = user.firstName.trim();
    const firstNameLabel = tCommon('labels.first_name');
    const { isRequired: firstNameIsRequired, minLength: firstNameMinLength, maxLength: firstNameMaxLength } = userValidation.firstName;

    if (firstNameIsRequired && !firstName) {
        errors.firstName = errorMessages.required(usernameLabel);
    } else if (!(firstName.length >= firstNameMinLength && firstName.length <= firstNameMaxLength)) {
        errors.firstName = errorMessages.length(firstNameLabel, firstNameMinLength, firstNameMaxLength);
    }
    
    const lastName = user.lastName.trim();
    const lastNameLabel = tCommon('labels.last_name');
    const { isRequired: lastNameIsRequired, minLength: lastNameMinLength, maxLength: lastNameMaxLength } = userValidation.firstName;

    if (lastNameIsRequired && !lastName) {
        errors.lastName = errorMessages.required(lastNameLabel);
    } else if (!(lastName.length >= lastNameMinLength && lastName.length <= lastNameMaxLength)) {
        errors.lastName = errorMessages.length(lastNameLabel, lastNameMinLength, lastNameMaxLength);
    }

    const email = user.email.trim();
    const emailLabel = tCommon('labels.email');
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
    const passwordLabel = tCommon('labels.password');
    const { isRequired: passwordIsRequired, minLength: passwordMinLength, maxLength: passwordMaxLength } = userValidation.password;

    if (passwordIsRequired && !password) {
        errors.password = errorMessages.required(passwordLabel);
    } else if (!(password.length >= passwordMinLength && password.length <= passwordMaxLength)) {
        errors.password = errorMessages.length(passwordLabel, passwordMinLength, passwordMaxLength);
    }

    const confirmPassword = user.confirmPassword.trim();
    const confirmPasswordLabel = tCommon('labels.confirm_password');
    const { isRequired: confirmPasswordIsRequired } = userValidation.confirmPassword;

    if (confirmPasswordIsRequired && !confirmPassword) {
        errors.confirmPassword = errorMessages.required(confirmPasswordLabel);
    } else if (confirmPassword !== user.password.trim()) {
        errors.confirmPassword = tCommon('errors.equal');
    }

    return errors;
};