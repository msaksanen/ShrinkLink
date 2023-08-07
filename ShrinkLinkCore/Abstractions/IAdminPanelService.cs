using ShrinkLinkCore.TableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.Abstractions
{
    public interface IAdminPanelService
    {
        Task<UrlManagerModel?> GenerateURLManagerModel(UrlMgrInpObj inpObj, string userId);
    }
}
