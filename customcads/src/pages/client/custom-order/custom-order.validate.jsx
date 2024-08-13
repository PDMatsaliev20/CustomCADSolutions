import { useTranslation } from 'react-i18next';
import useErrors from '@/hooks/useErrors';
import orderValidation from '@/constants/data/order';

export default (order) => {
    const { t } = useTranslation();
    const errorMessages = useErrors();
    let errors = {};

    const name = order.name.trim();
    const nameLabel = t('common.labels.name');
    const { isRequired: nameIsRequired, minLength: nameMinLength, maxLength: nameMaxLength } = orderValidation.name;

    if (nameIsRequired && !name) {
        errors.name = errorMessages.required(nameLabel);
    } else if (!(name.length >= nameMinLength && name.length <= nameMaxLength)) {
        errors.name = errorMessages.length(nameLabel, nameMinLength, nameMaxLength);
    }

    const description = order.description.trim();
    const descriptionLabel = t('common.labels.description');
    const { isRequired: descriptionIsRequired, minLength: descriptionMinLength, maxLength: descriptionMaxLength } = orderValidation.description;

    if (descriptionIsRequired && !description) {
        errors.description = errorMessages.required(descriptionLabel);
    } else if (!(description.length >= descriptionMinLength && description.length <= descriptionMaxLength)) {
        errors.description = errorMessages.length(descriptionLabel, descriptionMinLength, descriptionMaxLength);
    } 

    const categoryId = order.categoryId;
    const categoryLabel = t('common.labels.category');
    const { isRequired: categoryIdIsRequired } = orderValidation.categoryId;

    if (categoryIdIsRequired && !categoryId) {
        errors.categoryId = errorMessages.required(categoryLabel);
    }

    return errors;
};