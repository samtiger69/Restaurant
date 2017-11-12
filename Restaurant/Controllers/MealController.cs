using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// meal list/get/create/update
    /// </summary>
    public class MealController : BaseController
    {
        /// <summary>
        /// get meal list
        /// </summary>
        /// <returns>basic meal info list</returns>
        [Authorize]
        [HttpPost]
        public Response<List<Meal>> List(Request<MealList> request)
        {
            try
            {
                return new Response<List<Meal>>()
                {
                    Data = Cache.GetMeals(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<List<Meal>>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<List<Meal>>()
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
        /// get meal details
        /// </summary>
        /// <returns>MealAttributes/mealAttributeGroups/MealImages</returns>
        [Authorize]
        [HttpPost]
        public Response<MealInfo> Get(Request<int> request)
        {
            try
            {
                return new Response<MealInfo>()
                {
                    Data = Cache.GetMealInfo(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<MealInfo>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<MealInfo>()
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
        /// create a meal
        /// </summary>
        /// <returns>created meal id</returns>
        [Authorize(Roles = "admin, branch_admin")]
        [HttpPost]
        public Response<int> Create(Request<MealCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateMealCreate(request.Data);
                var mealService = MealService.GetInstance();
                return mealService.Create(request);
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
        /// update a meal
        /// </summary>
        [Authorize(Roles = "admin, branch_admin")]
        [HttpPost]
        public Response Update(Request<MealUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var mealService = MealService.GetInstance();
                return mealService.Update(request);
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
