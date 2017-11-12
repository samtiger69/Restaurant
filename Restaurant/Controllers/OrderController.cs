using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// orders list/create/update
    /// </summary>
    public class OrderController : BaseController
    {
        /// <summary>
        /// list orders
        /// </summary>
        /// <returns>list of orders</returns>
        [Authorize]
        [HttpPost]
        public Response<List<Branch>> List(Request<BaseList> request)
        {
            try
            {
                ValidateBaseRequest(request,true);
                return new Response<List<Branch>>()
                {
                    Data = Cache.GetBranches(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<List<Branch>>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<List<Branch>>()
                {
                    Data = null,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }

        /// <summary>
        /// create an order
        /// </summary>
        /// <returns>created order id</returns>
        [Authorize]
        [HttpPost]
        public Response<int> Create(Request<OrderCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateCreateOrder(request.Data);
                var orderService = OrderService.GetInstance();
                return orderService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<int>
                {
                    Data = -1,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<int>
                {
                    Data = -1,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }

        /// <summary>
        /// change order status
        /// </summary>
        [Authorize(Roles = "branch_admin, delivery_man")]
        [HttpPost]
        public Response Update(Request<OrderUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var orderService = OrderService.GetInstance();
                return orderService.Update(request);
            }
            catch (RestaurantException ex)
            {
                return new Response
                {
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response
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
