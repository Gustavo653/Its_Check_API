namespace ItsCheck.Domain
{
    public class Item : TenantBaseEntity
    {
        public required string Name { get; set; }
        public int? ParentItemId { get; set; }
        public virtual Item? ParentItem { get; set; }
        public virtual List<Item>? Items { get; set; }
    }
}
