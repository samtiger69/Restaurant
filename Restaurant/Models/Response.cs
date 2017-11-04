namespace Restaurant.Models
{
    public class Response
    {
        public ErrorCode ErrorCode { get; set; }
    }
    public class Response<T> : Response
    {
        public T Data { get; set; }
    }
}