namespace Restaurant.Models
{
    /// <summary>
    /// attribute update model
    /// </summary>
    public class AttributeUpdate : BaseUpdate
    {
        /// <summary>
        /// to remove group send "-1"
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// attribute price
        /// </summary>
        public decimal? Price { get; set; }
    }
}