using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Common.Models;

public class PageModel<T>
{
    public int CurrentPage { get; set; }              
    public int PageSize { get; set; }          
    public int TotalItems { get; set; }        
    public int TotalPages { get; set; }        
    public IEnumerable<T> Items { get; set; }  
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public PageModel() { }

    public PageModel(IEnumerable<T> data, int page, int pageSize, int totalItems)
    {
        Items = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    public PageModel<R> Map<R>(Func<T, R> map)
    {
        return new PageModel<R> { 
        Items = Items.Select(map),
        CurrentPage = CurrentPage,
        PageSize = CurrentPage,
        TotalItems = TotalItems,
        TotalPages = TotalPages
        };
    }
}
