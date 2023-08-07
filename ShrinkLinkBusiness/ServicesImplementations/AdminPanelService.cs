using AutoMapper;
using Azure;
using Humanizer;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCore.TableObjects;
using ShrinkLinkCQS.Links.Queries;
using ShrinkLinkCQS.Users.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShrinkLinkBusiness.ServicesImplementations
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private int _pageSize = 7;

        public AdminPanelService(IConfiguration configuration, IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _configuration = configuration;
            _mediator = mediator;
        }
        public async Task<UrlManagerModel?> GenerateURLManagerModel(UrlMgrInpObj inpObj, string userId)
        {
            var res = Guid.TryParse(userId, out Guid UId);
            
            if (!res)
                return null;

            var user = await _mediator.Send(new GetUserByIdQuery() { UserId = UId });

            if (user == null)
                return null;

           var list = await _mediator.Send(new GetLinksByUserIdQuery() { UserId =user.Id });

            if (list == null)
                return null;

            bool result = int.TryParse(_configuration["PageSize:Default"], out var pageSize);
            if (result) _pageSize = pageSize;
            int intCount = 0;

            if (list != null && list.Any())
            {
                list = URLFilter(list, inpObj);
                list = URLSort(list, inpObj.SortOrder);
            }
            var count = list?.Count();
            if (count != null)
                intCount = (int)count;
            IEnumerable<LinkDto>? pageItems = list?.Skip((inpObj.Page - 1) * _pageSize).Take(_pageSize).ToList();
            if(pageItems==null) return null;
            string pageRoute = @"/userpanel/urlmanager?page=";
            string processOptions = $"&url={inpObj.URL}&shortid={inpObj.ShortId}&expdate{inpObj.ExpDate}&sortorder={inpObj.SortOrder}";

            UrlManagerModel model = new(pageItems, processOptions,
                                    new PageModel(intCount,inpObj.Page, _pageSize, pageRoute),
                                    new UrlFilterModel(inpObj.URL, inpObj.ShortId, inpObj.ExpDate),
                                    new UrlSortModel(inpObj.SortOrder));
            return model;
        }

        internal IEnumerable<LinkDto> URLFilter (IEnumerable<LinkDto> list, UrlMgrInpObj inp)
        {
            if (!string.IsNullOrEmpty(inp.ExpDate))
            {
                var expTime = DateOnly.TryParse(inp.ExpDate, out DateOnly expDate);
                if (expTime)
                {
                    var expUpper = expDate.AddDays(1);

                    var expDateTimeLowLimit = expDate.ToDateTime(TimeOnly.MinValue);
                    var expDateUpLowLimit = expUpper.ToDateTime(TimeOnly.MinValue);
                    list = list.Where(a => a.ExpirationDate >= expDateTimeLowLimit && a.ExpirationDate <= expDateUpLowLimit);
                }
            }
            if (!string.IsNullOrEmpty(inp.URL))
            {
                list= list.Where(l => l.URL!= null && l.URL.Equals(inp.URL, StringComparison.InvariantCulture));
            }

            if (!string.IsNullOrEmpty(inp.ShortId))
            {
                list = list.Where(l => l.ShortId != null && l.ShortId.Equals(inp.ShortId, StringComparison.InvariantCulture));
            }

            return list;
        }

        internal IEnumerable<LinkDto> URLSort(IEnumerable<LinkDto> list, SortState sortOrder)
        {
            switch (sortOrder)
            {
                case SortState.URLAsc:
                    list = list.OrderBy(l => l.URL); break;
                case SortState.URLDesc:
                    list = list.OrderByDescending(l => l.URL); break;
                case SortState.ShortUrlAsc:
                    list = list.OrderBy(l => l.ShortId); break;
                case SortState.ShortUrlDesc:
                    list = list.OrderByDescending(l => l.ShortId); break;
                case SortState.ExpirationDateDesc:
                    list = list.OrderByDescending(l => l.ExpirationDate); break;
                case SortState.CreationDateAsc:
                    list = list.OrderBy(l => l.CreationDate); break;
                case SortState.CreationDateDesc:
                    list = list.OrderByDescending(l => l.CreationDate); break;
                case SortState.CountAsc:
                    list = list.OrderBy(l => l.Counter); break;
                case SortState.CountDesc:
                    list = list.OrderByDescending(l => l.Counter); break;
                default:
                    list = list.OrderBy(l => l.ExpirationDate); break;
            }
            return list;

        }
    }
}
