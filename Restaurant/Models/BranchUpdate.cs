namespace Restaurant.Models
{
    /// <summary>
    /// branch update model
    /// </summary>
    public class BranchUpdate : BaseUpdate
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