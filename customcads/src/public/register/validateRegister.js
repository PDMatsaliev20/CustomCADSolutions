import userValidation from '@/constants/data/user'
import * as errorMessages from '@/constants/errors' 

export default (user) => {
    let errors = { username: '', email: '', password: '', confirmPassword: '' };
    const username = user.username.trim();
    const { isRequired: usernameIsRequired, minLength: usernameMinLength, maxLength: usernameMaxLength } = userValidation.username;

    if (usernameIsRequired && !username) {
        errors.username = errorMessages.required('Username');
    } else if (!(username.length >= usernameMinLength && username.length <= usernameMaxLength)) {
        errors.username = errorMessages.length('Username', usernameMinLength, usernameMaxLength);
    }

    const email = user.email.trim();
    const { isRequired: emailIsRequired, minLength: emailMinLength, maxLength: emailMaxLength, regex: emailRegex } = userValidation.email;

    if (emailIsRequired && !email) {
        errors.email = errorMessages.required('Email');
    } else if (email.length > emailMaxLength) {
        errors.email = errorMessages.length('Email', emailMinLength, emailMaxLength);
    } else if (!email.includes('@')) {
        errors.email = errorMessages.requiredSymbol('Email', '@');
    } else if (email.startsWith('@')) {
        errors.email = errorMessages.invalidFirstChar('@');
    } else if (email.endsWith('@')) {
        errors.email = errorMessages.invalidLastChar('@');
    } else if (!emailRegex.test(email)) {
        errors.email = errorMessages.invalidPattern('Email');
    }

    const password = user.password.trim();
    const { isRequired: passwordIsRequired, minLength: passwordMinLength, maxLength: passwordMaxLength } = userValidation.password;

    if (passwordIsRequired && !password) {
        errors.password = errorMessages.required('Password');
    } else if (!(password.length >= passwordMinLength && password.length <= passwordMaxLength)) {
        errors.password = errorMessages.length('Password', passwordMinLength, passwordMaxLength);
    }

    const confirmPassword = user.confirmPassword.trim();
    const { isRequired: confirmPasswordIsRequired } = userValidation.confirmPassword;

    if (confirmPasswordIsRequired && !confirmPassword) {
        errors.confirmPassword = errorMessages.required('Password');
    } else if (confirmPassword !== user.password.trim()) {
        errors.confirmPassword = 'Passwords must be equal!';
    }

    return errors;
};