export default interface GalleryPageCad {
    id: number
    name: string
    imagePath: string
    creationDate: string
    creatorName: string
};

export const emptyGalleryPageCad: GalleryPageCad = {
    id: 0,
    name: '',
    imagePath: '',
    creationDate: '',
    creatorName: '',
};