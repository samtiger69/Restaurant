using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// mealtype list/create/update
    /// </summary>
    public class MealTypeController : BaseController
    {
        /// <summary>
        /// list meal types
        /// </summary>
        /// <returns>list of meal types</returns>
        [Authorize]
        [HttpPost]
        public Response<List<MealType>> List(Request<MealTypeList> request)
        {
            try
            {
                return new Response<List<MealType>>()
                {
                    Data = Cache.GetMealTypes(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<List<MealType>>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<List<MealType>>()
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
        /// create meal type
        /// </summary>
        /// <returns>created meal type id</returns>
        [Authorize(Roles = "admin, branch_admin")]
        [HttpPost]
        public Response<int> Create(Request<MealTypeCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateMealTypeCreate(request.Data);
                var mealType = MealTypeService.GetInstance();
                return mealType.Create(request);
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
        /// update meal type
        /// </summary>
        [Authorize(Roles = "branch_admin")]
        [HttpPost]
        public Response Update(Request<MealTypeUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var mealTypeService = MealTypeService.GetInstance();
                return mealTypeService.Update(request);
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
