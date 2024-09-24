import order from '@/constants/data/order';
import translate from '@/utils/translate';

export default () => {
    const tCommon = translate('common');

const { name, description, categoryId } = order;

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

return { name: nameValidation, description: descriptionValidation, categoryId: categoryIdValidation };
};