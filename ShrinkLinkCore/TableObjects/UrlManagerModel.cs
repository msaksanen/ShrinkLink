using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.TableObjects
{
    public class UrlManagerModel
    {
        public IEnumerable<LinkDto> Links { get; }
        public PageModel PageModel { get; }
        public UrlFilterModel FilterModel { get; }
        public UrlSortModel SortModel { get; }
        public string ProcessOptions { get; } = string.Empty;
        public string SystemInfo { get; set; } = string.Empty;

        public UrlManagerModel(IEnumerable<LinkDto> links, string processOptions,
            PageModel pageModel, UrlFilterModel filterModel, UrlSortModel sortModel)
        {
            Links = links;
            PageModel = pageModel;
            FilterModel = filterModel;
            SortModel = sortModel;
            ProcessOptions = processOptions;

        }
    }
}
