import { useTranslation } from 'react-i18next';
import cad from '@/constants/data/cad';

export default () => {
    const { t: tCommon } = useTranslation('common');

    const { name, description, categoryId, price } = cad;

    const nameInfo = { field: tCommon('labels.name'), max: name.maxLength, min: name.minLength };
    const nameValidation = {
        required: {
            value: name.isRequired,
            message: tCommon('errors.required', nameInfo),
        },
        minLength: {
            value: name.minLength,
            message: tCommon('errors.length', nameInfo),
        },
        maxLength: {
            value: name.maxLength,
            message: tCommon('errors.length', nameInfo),
        }
    };

    const descriptionInfo = { field: tCommon('labels.description'), max: description.maxLength, min: description.minLength };
    const descriptionValidation = {
        required: {
            value: description.isRequired,
            message: tCommon('errors.required', descriptionInfo),
        },
        minLength: {
            value: description.minLength,
            message: tCommon('errors.length', descriptionInfo),
        },
        maxLength: {
            value: description.maxLength,
            message: tCommon('errors.length', descriptionInfo),
        }
    };

    const categoryIdInfo = { field: tCommon('labels.category') };
    const categoryIdValidation = {
        required: {
            value: categoryId.isRequired,
            message: tCommon('errors.required', categoryIdInfo),
        }
    };
    
    const priceInfo = { field: tCommon('labels.price'), min: price.min, max: price.max };
    const priceValidation = {
        required: {
            value: price.isRequired,
            message: tCommon('errors.required', priceInfo),
        },
        min: {
            value: price.min,
            message: tCommon('errors.range', priceInfo),
        },
        max: {
            value: price.max,
            message: tCommon('errors.range', priceInfo)
        }
    };

    return { name: nameValidation, description: descriptionValidation, categoryId: categoryIdValidation, price: priceValidation };
};