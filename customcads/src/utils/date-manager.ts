const dateToMachineReadable = (date: string): string => {
    if (date) {
        return date.replaceAll('.', '-').replaceAll('/', '-').replaceAll('\\', '-');
    } else return date;
};

export { dateToMachineReadable };