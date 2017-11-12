using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// base request class
    /// </summary>
    public class Request
    {
        /// <summary>
        /// logged in user id
        /// </summary>
        public string UserId { get; set; }
    }

    /// <summary>
    /// bas request with T data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Request<T> : Request
    {
        /// <summary>
        /// generic data object
        /// </summary>
        [Required]
        public T Data { get; set; }
    }
}