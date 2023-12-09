namespace ItsCheck.Domain
{
    public class Ambulance : TenantBaseEntity
    {
        public required int Number { get; set; }
        public required virtual Checklist Checklist { get; set; }
    }
}
