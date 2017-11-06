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
    public class MealTypeController : BaseController
    {
        [Authorize]
        public Response<List<MealType>> List(Request<MealTypeListModel> request)
        {
            try
            {
                return new Response<List<MealType>>()
                {
                    Data = Cache.GetMealTypes(request.Data),
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response<MealType> Create(Request<MealType> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response Update(Request<MealTypeUpdateModel> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
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
