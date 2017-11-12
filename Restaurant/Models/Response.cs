namespace Restaurant.Models
{
    /// <summary>
    /// base response class
    /// </summary>
    public class Response
    {
        /// <summary>
        /// response error
        /// </summary>
        public ErrorCode ErrorCode { get; set; }
    }

    /// <summary>
    /// base reponse of T data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response
    {
        /// <summary>
        /// generic object T
        /// </summary>
        public T Data { get; set; }
    }
}