using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class BranchController : BaseController
    {
        [CustomAuthorization]
        [HttpPost]
        public Response<List<Branch>> List(Request<BaseListModel> request)
        {
            try
            {
                return new Response<List<Branch>>()
                {                    
                    Data = Cache.GetBranches(request.Data),
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response<Branch> Create(Request<Branch> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                var branchService = BranchService.GetInstance();
                TrimNames(request.Data);
                ValidateNames(request.Data);
                return branchService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<Branch>
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<Branch>
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response Update(Request<Branch> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                var branchService = BranchService.GetInstance();
                return branchService.Update(request);
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
