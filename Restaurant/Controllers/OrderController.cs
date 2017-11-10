using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class OrderController : BaseController
    {
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

        [Authorize(Roles = "admin, branch_admin, delivery_man")]
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
