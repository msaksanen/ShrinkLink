using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.DataTransferObjects
{
    public class Result
    {
        public string? Text { get; set; }
        public int Length { get; set; } = 0;
        public int CountResult { get; set; } = 0;
        public int? SaveResult { get; set; } = 0;
        public bool BoolResult { get; set; } = true;
    }
}
