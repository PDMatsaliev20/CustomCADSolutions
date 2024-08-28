const dateToMachineReadable = (date) => {
    if (date) {
        return date.replaceAll('.', '-').replaceAll('/', '-').replaceAll('\\', '-');
    } else return date;
};

export { dateToMachineReadable };