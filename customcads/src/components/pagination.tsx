import { useTranslation } from 'react-i18next';
import ArrowBtn from './pagination/arrow-btn';
import PageNum from './pagination/page-num';

interface PaginationProps {
    page: number
    onPageChange: { (newPage: number): void }
    limit: number
    total: number
}

function Pagination({ page, onPageChange, limit, total }: PaginationProps) {
    const { t: tCommon } = useTranslation('common');

    const lastPage = Math.ceil(total / limit);

    const handleBeginning = () => {
        onPageChange(1);
    }
    const handlePrevious = () => {
        onPageChange(page - 1);
    }
    const handleNext = () => {
        onPageChange(page + 1);
    }
    const handleEnd = () => {
        onPageChange(lastPage);
    }

    const pageNums = [];
    for (let i = 1; i <= lastPage; i++) {
        const pageNum = <PageNum key={i} num={i} onClick={() => onPageChange(i)} isCurrent={page === i} />;
        pageNums.push(pageNum);
    }

    return (
        <div className="flex items-center justify-between px-4 py-8 bg-indigo-100 text-indigo-900 font-bold rounded-lg border-2 border-indigo-400 shadow-xl shadow-indigo-300 sm:px-6">
            <div className="flex flex-1 justify-between sm:hidden">
                <button onClick={handlePrevious}
                    className="relative inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
                >
                    {tCommon('pagination.previous')}
                </button>
                <button onClick={handleNext}
                    className="relative ml-3 inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
                >
                    {tCommon('pagination.next')}
                </button>
            </div>
            <div className="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
                <p>{
                    tCommon('pagination.message', {
                        total: total,
                        start: limit * (page - 1) + 1,
                        end: page * limit < total ? page * limit : total,
                    })
                }</p>
                <div aria-label="Pagination" className="isolate inline-flex -space-x-px rounded-md shadow-sm">
                    <ArrowBtn text={tCommon('pagination.beginning')} type="beginning" onClick={handleBeginning} />
                    <ArrowBtn text={tCommon('pagination.previous')} type="previous" onClick={handlePrevious} />
                    {pageNums}
                    <ArrowBtn text={tCommon('pagination.next')} type="next" onClick={handleNext} />
                    <ArrowBtn text={tCommon('pagination.end')} type="end" onClick={handleEnd} />
                </div>
            </div>
        </div>
    )
}

export default Pagination; 