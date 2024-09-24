export default {
    name: {
        isRequired: true,
        maxLength: 18,
        minLength: 2,
    },
    description: {
        isRequired: true,
        maxLength: 750,
        minLength: 5,
    },
    categoryId: {
        isRequired: true,
    },

};