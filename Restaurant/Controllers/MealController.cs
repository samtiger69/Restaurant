using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class MealController : BaseController
    {
        [Authorize]
        public Response<List<Meal>> List(Request<MealListModel> request)
        {
            try
            {
                return new Response<List<Meal>>()
                {
                    Data = Cache.GetMeals(request.Data),
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response<Meal> Create(Request<Meal> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                var mealService = MealService.GetInstance();
                TrimNames(request.Data);
                ValidateNames(request.Data);
                return mealService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<Meal>
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<Meal>
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
        public Response Update(Request<UpdateMealModel> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
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
