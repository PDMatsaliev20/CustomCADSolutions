import orderValidation from '@/constants/data/order'
import * as errorMessages from '@/constants/errors'

export default (order) => {
    let errors = {};

    const name = order.name.trim();
    const { isRequired: nameIsRequired, minLength: nameMinLength, maxLength: nameMaxLength } = orderValidation.name;

    if (nameIsRequired && !name) {
        errors.name = errorMessages.required('Name');
    } else if (!(name.length >= nameMinLength && name.length <= nameMaxLength)) {
        errors.name = errorMessages.length('Name', nameMinLength, nameMaxLength);
    }

    const description = order.description.trim();
    const { isRequired: descriptionIsRequired, minLength: descriptionMinLength, maxLength: descriptionMaxLength } = orderValidation.description;

    if (descriptionIsRequired && !description) {
        errors.description = errorMessages.required('Description');
    } else if (description.length > descriptionMaxLength) {
        errors.description = errorMessages.length('Description', descriptionMinLength, descriptionMaxLength);
    } 

    const categoryId = order.categoryId;
    const { isRequired: categoryIdIsRequired } = orderValidation.categoryId;

    if (categoryIdIsRequired && !categoryId) {
        errors.categoryId = errorMessages.required('Category');
    }

    return errors;
};