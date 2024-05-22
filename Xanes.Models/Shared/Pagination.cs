namespace Xanes.Models.Shared;
public class Pagination
{
    private const int MaxPageSize = 50;
    private int _pageSize = 15;

    public int pageNumber { get; set; }

    public int pageSize
    {
        get { return _pageSize; }
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public int totalPages { get; set; } = 0;
    public int totalCount { get; set; } = 0;
    public bool hasPrevious => pageNumber > 1;
}
