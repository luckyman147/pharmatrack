namespace PharmaTrack.Shared.Pagination;

    public  class PaginationResult<TEntity>
        (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data) where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
    public int PagesReste { get; } = (int)Math.Ceiling((double) count / pageSize) - pageIndex -1 ;
        public IEnumerable<TEntity> Data { get; } = data;
    }
