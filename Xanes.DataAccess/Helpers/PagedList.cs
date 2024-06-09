using Microsoft.EntityFrameworkCore;

namespace Xanes.DataAccess.Helpers;
public class PagedList<T> : List<T>
{
    private readonly int _maxPageSize = 50;
    private int _recordsPage = 10;

    public int PageNumber { get; private set; }

    public int PageSize { 
        get => _recordsPage;
        set => _recordsPage = (value > _maxPageSize) ? _maxPageSize : value;
    }

    public int TotalPages { get; set; } = 0;
    public int TotalCount { get; set; } = 0;
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        PageNumber = pageNumber;
        if (pageSize == 0)
        {
            TotalPages = 1;
        }
        else
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        AddRange(items);
    }

    public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, bool? isList = null)
    {
        List<T> items;
        pageSize = Math.Min(pageSize, 50);

        var count = source.Count();

        if (isList == null || isList == false)
        {
            if (pageNumber == 1 && pageSize == 0)
            {
                items = await source
                    .ToListAsync();
            }
            else
            {
                items = await source
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }
        else
        {

            if (pageNumber == 1 && pageSize == 0)
            {
                items =  source
                    .ToList();
            }
            else
            {
                items = source
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
