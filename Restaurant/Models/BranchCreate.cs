namespace Restaurant.Models
{
    /// <summary>
    /// branch create model
    /// </summary>
    public class BranchCreate : BaseCreate
    {
        /// <summary>
        /// location description text
        /// </summary>
        public string LocationDescription { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public string Longitude { get; set; }
    }
}