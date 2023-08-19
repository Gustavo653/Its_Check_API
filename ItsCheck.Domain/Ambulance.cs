using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.Domain
{
    public class Ambulance : BaseEntity
    {
        public int Number { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
