namespace EShop.ViewModels
{
    public class PaginationViewModel<T>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
        public int Total { get; set; } 

        public List<T> Data { get; set; }



    }
    //PageCount = Total 
    //canNext = PageNumber * PageSize < Total  
    //canback = PageNumber > 0
}
