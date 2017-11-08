namespace Restaurant.Entities
{
    public class BasicEntity : BaseEntity
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}