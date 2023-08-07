using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkDb.Entities
{
    public class Link : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? URL { get; set; }
        public string? Hash { get; set; }
        public string? ShortId { get; set; }
        public int? Counter { get; set; } = 0;
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<User>? Users { get; set;}

    }
}
