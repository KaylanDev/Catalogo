namespace Catalogo.Paginations
{
    public class PagedList<T>:List<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPage;

        public PagedList(List<T>itens, int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;
            TotalPage = (int)Math.Ceiling(count /(double) pageSize); 
            AddRange (itens);
        }

        public static PagedList<T> TopagedList(IQueryable<T> source,int pageNumber,int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items,count,pageNumber,pageSize);
        }

    }
}
