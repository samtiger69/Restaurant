﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public string GetGreeting(string txt)
        {
            return txt + " from server";
        }
    }
}
