namespace Restaurant.Models
{
    public class Request
    {
        public string Token { get; set; }
        public int UserId { get; set; }

    }

    public class Request<T> : Request
    {
        public T Data { get; set; }
    }
}