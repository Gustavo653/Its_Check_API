using ItsCheck.Domain.Enum;
using ItsCheck.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.Domain
{
    public class ChecklistAdjustedItem : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual Checklist Checklist { get; set; }
        public virtual Item Item { get; set; }
    }
}
