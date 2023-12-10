using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.Domain
{
    public class Tenant : BaseEntity
    {
        public required string Name { get; set; }
    }
}
