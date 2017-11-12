namespace Restaurant.Models
{
    /// <summary>
    /// base list model
    /// </summary>
    public class BaseList
    {
        /// <summary>
        /// object id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// english name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// arabic name
        /// </summary>
        public string NameAr { get; set; }
    }
}