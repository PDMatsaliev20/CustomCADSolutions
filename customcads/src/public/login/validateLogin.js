import userValidation from '@/constants/data/user'
import * as errorMessages from '@/constants/errors' 

export default (user) => {
    let errors = {};
    const username = user.username.trim();
    const { isRequired: usernameIsRequired, minLength: usernameMinLength, maxLength: usernameMaxLength } = userValidation.username;

    if (usernameIsRequired && !username) {
        errors.username = errorMessages.required('Username');
    } else if (!(username.length >= usernameMinLength && username.length <= usernameMaxLength)) {
        errors.username = errorMessages.length('Username', usernameMinLength, usernameMaxLength);
    }

    const password = user.password.trim();
    const { isRequired: passwordIsRequired, minLength: passwordMinLength, maxLength: passwordMaxLength } = userValidation.password;

    if (passwordIsRequired && !password) {
        errors.password = errorMessages.required('Password');
    } else if (!(password.length >= passwordMinLength && password.length <= passwordMaxLength)) {
        errors.password = errorMessages.length('Password', passwordMinLength, passwordMaxLength);
    }

    return errors;
};