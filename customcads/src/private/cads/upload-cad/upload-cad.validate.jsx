import { useTranslation } from 'react-i18next';
import useErrors from '@/hooks/useErrors';
import cadValidation from '@/constants/data/cad';

export default (cad) => {
    const { t } = useTranslation();
    const errorMessages = useErrors();
    let errors = {};

    const name = cad.name.trim();
    const nameLabel = t('common.labels.Name');
    const { isRequired: nameIsRequired, minLength: nameMinLength, maxLength: nameMaxLength } = cadValidation.name;
    if (nameIsRequired && !name) {
        errors.name = errorMessages.required(nameLabel);
    } else if (!(name.length >= nameMinLength && name.length <= nameMaxLength)) {
        errors.name = errorMessages.length(nameLabel, nameMinLength, nameMaxLength);
    }

    const description = cad.description.trim();
    const descriptionLabel = t('common.labels.Description');
    const { isRequired: descriptionIsRequired, minLength: descriptionMinLength, maxLength: descriptionMaxLength } = cadValidation.description;

    if (descriptionIsRequired && !description) {
        errors.description = errorMessages.required(descriptionLabel);
    } else if (!(description.length >= descriptionMinLength && description.length <= descriptionMaxLength)) {
        errors.description = errorMessages.length(descriptionLabel, descriptionMinLength, descriptionMaxLength);
    } 

    const categoryId = cad.categoryId;
    const categoryLabel = t('common.labels.Category');
    const { isRequired: categoryIdIsRequired } = cadValidation.categoryId;

    if (categoryIdIsRequired && !categoryId) {
        errors.categoryId = errorMessages.required(categoryLabel);
    }

    const price = cad.price;
    const priceLabel = t('common.labels.Price');
    const { isRequired: priceIsRequired, min: priceMinVal, max: priceMaxVal } = cadValidation.price;

    if (priceIsRequired && !price) {
        errors.price = errorMessages.required(priceLabel);
    } else if (price < priceMinVal || price > priceMaxVal) {
        errors.price = errorMessages.range(priceLabel, priceMinVal, priceMaxVal) + '$';
    }

    const file = cad.file;
    const fileLabel = t('common.labels.3D Model');
    const { isRequired: fileIsRequired } = cadValidation.file;

    if (fileIsRequired && !file) {
        errors.file = errorMessages.required(fileLabel);
    }

    const image = cad.image;
    const imageLabel = t('common.labels.Image');
    const { isRequired: imageIsRequired } = cadValidation.image;

    if (imageIsRequired && !image) {
        errors.image = errorMessages.required(imageLabel);
    }

    return errors;
};