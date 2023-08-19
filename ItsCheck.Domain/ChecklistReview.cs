using ItsCheck.Domain.Enum;
using ItsCheck.Domain.Identity;

namespace ItsCheck.Domain
{
    public class ChecklistReview : BaseEntity
    {
        public ReviewType Type { get; set; }
        public string Observation { get; set; }
        public virtual Ambulance Ambulance { get; set; }
        public virtual Checklist Checklist { get; set; }
        public virtual User User { get; set; }
    }
}
