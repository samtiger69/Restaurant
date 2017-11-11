using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using static Restaurant.Models.Enums;

namespace Restaurant.Controllers
{
    public class BaseController : ApiController
    {
        protected void ValidateNames(BasicEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Name))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Name cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (string.IsNullOrEmpty(entity.NameAr))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "NameAr cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void TrimNames(BasicEntity entity)
        {
            entity.Name = entity.Name.Trim();
            entity.NameAr = entity.NameAr.Trim();
        }

        protected void ValidateCreateOrder(OrderCreate orderCreate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCreate.UserId))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "UserId is required",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (orderCreate.BranchId < 0)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Incorrect branchId",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (orderCreate.OrderMeals == null || orderCreate.OrderMeals.Count < 1)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "OrderMeals cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
                if(!orderCreate.IsPickUp)
                {
                    if(orderCreate.Address == null)
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Address cannot be empty in case of delivery",
                                ErrorNumber = ErrorNumber.EmptyRequiredField
                            }
                        };
                    orderCreate.Address.Area = orderCreate.Address.Area.Trim();
                    orderCreate.Address.Street = orderCreate.Address.Street.Trim();
                    orderCreate.Address.Building = orderCreate.Address.Building.Trim();
                    orderCreate.Address.OfficeNumber = orderCreate.Address.OfficeNumber.Trim();
                }
                orderCreate.Notes = orderCreate.Notes.Trim();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateFields(ModelStateDictionary modelState)
        {
            try
            {
                if (!modelState.IsValid)
                {
                    var message = "";
                    foreach (var error in modelState.Values)
                    {
                        foreach (var violation in error.Errors)
                        {
                            message = violation.ErrorMessage + ",";
                        }
                        message = message.Substring(0, message.Length - 1);
                    }
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = message,
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
                }
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateBaseRequest(Request request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.UserId))
                throw new RestaurantException
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "Empty UserId",
                        ErrorNumber = ErrorNumber.EmptyRequiredField
                    }
                };
        }

        protected void ValidateBaseRequest<T>(Request<T> request, bool ignoreData = false)
        {
            if (request == null || (!ignoreData && request.Data == null))
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.UserId))
                throw new RestaurantException
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "Empty UserId",
                        ErrorNumber = ErrorNumber.EmptyRequiredField
                    }
                };
        }

        protected void ValidateImageCreate(ImageCreate request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Content))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "empty image content",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if(request.SourceId < 1)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "wrong source id",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (Enum.IsDefined(typeof(SourceType), request.SourceType))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "wrong source type",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateImageUpdate(ImageUpdate request)
        {
            try
            {
                if (request.Id < 1)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "wrong source id",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }
    }
}
