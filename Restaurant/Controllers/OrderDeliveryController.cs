﻿using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// order deliver create/update
    /// </summary>
    public class OrderDeliveryController : BaseController
    {
        /// <summary>
        /// create or update a deliver
        /// </summary>
        /// <returns>order delivery id</returns>
        [Authorize(Roles = "branch_admin")]
        [HttpPost]
        public Response<int> Save(Request<OrderDeliverySave> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var orderDeliveryService = OrderDeliveryService.GetInstance();
                return orderDeliveryService.Save(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<int>
                {
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<int>
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }
    }
}
