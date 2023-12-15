namespace ItsCheck.Domain
{
    public class AmbulanceChecklistXRef : TenantBaseEntity
    {
        public virtual int AmbulanceId { get; set; }
        public required virtual Ambulance Ambulance { get; set; }
        public virtual int ChecklistId { get; set; }
        public required virtual Checklist Checklist { get; set; }
    }
}
