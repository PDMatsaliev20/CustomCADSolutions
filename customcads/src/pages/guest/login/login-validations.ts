import { RegisterOptions } from 'react-hook-form';
import user from '@/constants/data/user';
import ILogin from '@/interfaces/login';
import translate from '@/utils/translate';

export default  () => {
    const tCommon = translate('common');
    
    const { username, password } = user;

    const usernameInfo = { field: tCommon('labels.username'), min: username.minLength, max: username.maxLength };
    const usernameValidation: RegisterOptions<ILogin, "username"> = {
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
    const passwordValidation: RegisterOptions<ILogin, "password"> = {
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
        password: passwordValidation,
    };
};