namespace Restaurant.Models
{
    /// <summary>
    /// meal update model
    /// </summary>
    public class MealUpdate : BaseUpdate
    {
        /// <summary>
        /// meal type id
        /// </summary>
        public int? MealTypeId { get; set; }

        /// <summary>
        /// meal price
        /// </summary>
        public decimal? Price { get; set; }
    }
}