using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.TableObjects
{
    public class UrlSortModel
    {
        public SortState URLSort { get; }
        public SortState ShortURLSort { get; }
        public SortState CountSort { get; }
        public SortState CreationDateSort { get; }
        public SortState ExpirationDateSort { get; }
        public SortState Current { get; }
        public SortState PrevState { get; }
        public bool Up { get; set; }

        public UrlSortModel(SortState sortOrder)
        {
            URLSort = SortState.URLAsc;
            ShortURLSort = SortState.ShortUrlAsc;
            CountSort = SortState.CountAsc;
            CreationDateSort = SortState.CreationDateAsc;
            ExpirationDateSort = SortState.ExpirationDateAsc;


            PrevState = sortOrder;

            Up = true;

            if (sortOrder == SortState.URLDesc || sortOrder == SortState.ShortUrlDesc || sortOrder == SortState.CountDesc ||
                sortOrder == SortState.CreationDateDesc || sortOrder == SortState.ExpirationDateDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.ExpirationDateAsc:
                    Current = ExpirationDateSort = SortState.ExpirationDateDesc;
                    break;
                case SortState.CreationDateAsc:
                    Current = CreationDateSort = SortState.CreationDateDesc;
                    break;
                case SortState.CreationDateDesc:
                    Current = CreationDateSort = SortState.CreationDateAsc;
                    break;
                case SortState.CountAsc:
                    Current = CreationDateSort = SortState.CountDesc;
                    break;
                case SortState.CountDesc:
                    Current = CreationDateSort = SortState.CountAsc;
                    break;
                case SortState.URLAsc:
                    Current = CreationDateSort = SortState.URLDesc;
                    break;
                case SortState.URLDesc:
                    Current = CreationDateSort = SortState.URLAsc;
                    break;
                case SortState.ShortUrlAsc:
                    Current = CreationDateSort = SortState.ShortUrlDesc;
                    break;
                case SortState.ShortUrlDesc:
                    Current = CreationDateSort = SortState.ShortUrlAsc;
                    break;

                default:
                    Current = ExpirationDateSort = SortState.ExpirationDateAsc;
                    break;


            }
        }
    }
}
