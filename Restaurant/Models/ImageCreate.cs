using static Restaurant.Models.Enums;

namespace Restaurant.Models
{
    public class ImageCreate
    {
        public int SourceId { get; set; }
        public SourceType SourceType { get; set; }
        public string Content { get; set; }
        public bool IsDefault { get; set; }
    }
}