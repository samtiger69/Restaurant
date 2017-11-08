using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Request
    {
        public string UserId { get; set; }

    }

    public class Request<T> : Request
    {
        public T Data { get; set; }
    }
}