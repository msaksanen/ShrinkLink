using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShrinkLinkDb.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<User>? Users { get; set; }      
    }
}

