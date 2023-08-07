using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.TableObjects
{
    public class PageModel
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public string PageRoute { get; } = string.Empty;


        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PageModel(int count, int pageNumber, int pageSize, string pageRoute)
        {
            PageNumber = pageNumber;
            PageRoute = pageRoute;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
