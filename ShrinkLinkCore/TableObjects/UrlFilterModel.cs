using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.TableObjects
{
    public class UrlFilterModel
    {
        public UrlFilterModel(string url, string shortid, string expDate)
        {
            SelectedURL = url;
            SelectedShortId = shortid;
            SelectedExpDate = expDate;
    }
        public string SelectedURL { get; }
        public string SelectedShortId { get;}
        public string SelectedExpDate { get; }
    }
}
