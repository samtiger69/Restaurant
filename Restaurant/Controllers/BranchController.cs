using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>  
    /// Branch CRUD operations controller
    /// </summary>
    public class BranchController : BaseController
    {
        /// <summary>  
        /// Returns list of branches
        /// </summary>
        [HttpPost]
        public Response<List<Branch>> List(Request<BaseList> request)
        {
            try
            {
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
        /// Create a branch
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response<int> Create(Request<BranchCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var branchService = BranchService.GetInstance();
                ValidateBranchCreate(request.Data);
                return branchService.Create(request);
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

        /// <summary>  
        /// Update a branch
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response Update(Request<BranchUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
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
