using static Restaurant.Models.Enums;

namespace Restaurant.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public SourceType SourceType { get; set; }
        public bool IsDefault { get; set; }
    }
}