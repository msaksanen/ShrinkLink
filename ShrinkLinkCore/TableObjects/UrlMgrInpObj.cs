using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.TableObjects
{
    public class UrlMgrInpObj
    {
        public string URL { get; set; } = string.Empty;
        public string ShortId { get; set; } = string.Empty;
        public string ExpDate { get; set; } = string.Empty;
        public SortState SortOrder { get; set; }
        public int Page { get; set; } = 1;
    }
}
