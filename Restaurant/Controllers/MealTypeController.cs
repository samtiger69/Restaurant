using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class MealTypeController : BaseController
    {
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

        [Authorize(Roles = "branch_admin")]
        [HttpPost]
        public Response<MealType> Create(Request<MealType> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var mealType = MealTypeService.GetInstance();
                TrimNames(request.Data);
                ValidateNames(request.Data);
                return mealType.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<MealType>
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<MealType>
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
