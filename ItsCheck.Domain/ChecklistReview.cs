using ItsCheck.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.Domain
{
    public class ChecklistReview : BaseEntity
    {
        public int Type { get; set; }
        public string Observation { get; set; }
        public virtual Ambulance Ambulance { get; set; }
        public virtual Checklist Checklist { get; set; }
        public virtual User User { get; set; }
    }
}
