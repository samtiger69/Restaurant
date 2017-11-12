namespace Restaurant.Models
{
    /// <summary>
    /// meal type update model
    /// </summary>
    public class MealTypeUpdate : BaseUpdate
    {
        /// <summary>
        /// optional description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// meal type branch id
        /// </summary>
        public int? BranchId { get; set; }
    }
}