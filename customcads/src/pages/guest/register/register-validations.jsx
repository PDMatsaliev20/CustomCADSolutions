import { useTranslation } from 'react-i18next';
import user from '@/constants/data/user';

export default (passwordValue) => {
    const { t: tCommon } = useTranslation('common');

    const { firstName, lastName, username, email, password, confirmPassword } = user;

    const firstNameInfo = { field: tCommon('labels.first_name'), min: firstName.minLength, max: firstName.maxLength };
    const firstNameValidation = {
        setValueAs: value => value === "" ? undefined : value,
        required: {
            value: firstName.isRequired,
            message: tCommon('errors.required', firstNameInfo),
        },
        minLength: {
            value: firstName.minLength,
            message: tCommon('errors.length', firstNameInfo),
        },
        maxLength: {
            value: firstName.maxLength,
            message: tCommon('errors.length', firstNameInfo),
        },
    };
    
    const lastNameInfo = { field: tCommon('labels.last_name'), min: lastName.minLength, max: lastName.maxLength };
    const lastNameValidation = {
        setValueAs: value => value === "" ? undefined : value,
        required: {
            value: lastName.isRequired,
            message: tCommon('errors.required', lastNameInfo),
        },
        minLength: {
            value: lastName.minLength,
            message: tCommon('errors.length', lastNameInfo),
        },
        maxLength: {
            value: lastName.maxLength,
            message: tCommon('errors.length', lastNameInfo),
        },
    };
    
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
    
    const emailInfo = { field: tCommon('labels.email'), min: email.minLength, max: email.maxLength };
    const emailValidation = {
        required: {
            value: email.isRequired,
            message: tCommon('errors.required', emailInfo),
        },
        minLength: {
            value: email.minLength,
            message: tCommon('errors.length', emailInfo),
        },
        maxLength: {
            value: email.maxLength,
            message: tCommon('errors.length', emailInfo),
        },
        pattern: {
            value: email.regex,
            message: tCommon('errors.pattern', emailInfo),
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
    
    const confirmPasswordInfo = { field: tCommon('labels.confirm_password') };
    const confirmPasswordValidation = {
        required: {
            value: confirmPassword.isRequired,
            message: tCommon('errors.required', confirmPasswordInfo),
        },
        validate: value => value === passwordValue || tCommon('errors.equal')
    };

    return {
        firstName: firstNameValidation,
        lastName: lastNameValidation,
        username: usernameValidation,
        email: emailValidation,
        password: passwordValidation,
        confirmPassword: confirmPasswordValidation
    };
};