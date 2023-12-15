namespace ItsCheck.Domain
{
    public class Ambulance : TenantBaseEntity
    {
        public required int Number { get; set; }
        public required string LicensePlate { get; set; }
        public virtual List<AmbulanceChecklistXRef>? AmbulanceChecklistXRefs { get; set; }
    }
}
