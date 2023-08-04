using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.Abstractions
{
    public interface ILinkService
    {
        Task<LinkGenObjExtend> GenerateShorLinkAsync(LinkDto dto);
    }
}
