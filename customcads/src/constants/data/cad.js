export default {
    name: {
        isRequired: true,
        maxLength: 18,
        minLength: 2,
    },
    description: {
        isRequired: true,
        maxLength: 750,
        minLength: 10,
    },
    categoryId: {
        isRequired: true,
    },
    price: {
        isRequired: true,
        min: 0.01,
        max: 1000
    },
    file: {
        isRequired: true
    },
    image: {
        isRequired: true
    }
};