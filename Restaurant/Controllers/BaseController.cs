using Restaurant.Models;
using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using static Restaurant.Models.Enums;

namespace Restaurant.Controllers
{
    public class BaseController : ApiController
    {

        protected void ValidateBranchCreate(BranchCreate branchCreate)
        {
            try
            {
                branchCreate.Name = branchCreate.Name.Trim();
                branchCreate.NameAr = branchCreate.NameAr.Trim();
                ValidateNames(branchCreate.Name, branchCreate.NameAr);
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateMealTypeCreate(MealTypeCreate mealTypeCreate)
        {
            try
            {
                mealTypeCreate.Name = mealTypeCreate.Name.Trim();
                mealTypeCreate.NameAr = mealTypeCreate.NameAr.Trim();
                ValidateNames(mealTypeCreate.Name, mealTypeCreate.NameAr);
                if (string.IsNullOrEmpty(mealTypeCreate.ImageContent))
                    ThrowError("Image is required", ErrorNumber.EmptyRequiredField);
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateMealCreate(MealCreate mealCreate)
        {
            try
            {
                mealCreate.Name = mealCreate.Name.Trim();
                mealCreate.NameAr = mealCreate.NameAr.Trim();
                ValidateNames(mealCreate.Name, mealCreate.NameAr);
                if (mealCreate.MealTypeId < 1)
                    ThrowError("wrong meal type id", ErrorNumber.EmptyRequiredField);

                if (mealCreate.MealImages == null || mealCreate.MealImages.Count < 1)
                    ThrowError("empty images", ErrorNumber.EmptyRequiredField);

                var hasDefault = false;
                foreach (var image in mealCreate.MealImages)
                {
                    if (image.IsDefualt)
                    {
                        hasDefault = true;
                        break;
                    }
                }
                if (!hasDefault)
                    ThrowError("no default image is specified", ErrorNumber.EmptyRequiredField);
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateAttributeCreate(Request<AttributeCreate> request)
        {
            try
            {
                request.Data.Name = request.Data.Name.Trim();
                request.Data.NameAr = request.Data.NameAr.Trim();
                ValidateNames(request.Data.Name, request.Data.NameAr);
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateAttributeGroupCreate(Request<AttributeGroupCreate> request)
        {
            try
            {
                request.Data.Name = request.Data.Name.Trim();
                request.Data.NameAr = request.Data.NameAr.Trim();
                ValidateNames(request.Data.Name, request.Data.NameAr);
                if (request.Data.Attributes == null || request.Data.Attributes.Count < 1)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "You must assign atributes to attributeGroup",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateOrderCreate(OrderCreate orderCreate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCreate.UserId))
                    ThrowError("UserId is required", ErrorNumber.EmptyRequiredField);

                if (orderCreate.BranchId < 0)
                    ThrowError("Incorrect branchId", ErrorNumber.EmptyRequiredField);

                if (orderCreate.OrderMeals == null || orderCreate.OrderMeals.Count < 1)
                    ThrowError("OrderMeals cannot be empty", ErrorNumber.EmptyRequiredField);

                if (!orderCreate.IsPickUp)
                {
                    if (orderCreate.Address == null || string.IsNullOrEmpty(orderCreate.Address.Area) || string.IsNullOrEmpty(orderCreate.Address.Building) || string.IsNullOrEmpty(orderCreate.Address.Floor) || string.IsNullOrEmpty(orderCreate.Address.OfficeNumber))
                        ThrowError("Address cannot be empty in case of delivery", ErrorNumber.EmptyRequiredField);
                    orderCreate.Address.Area = orderCreate.Address.Area.Trim();
                    orderCreate.Address.Street = orderCreate.Address.Street.Trim();
                    orderCreate.Address.Building = orderCreate.Address.Building.Trim();
                    orderCreate.Address.OfficeNumber = orderCreate.Address.OfficeNumber.Trim();

                    if (!string.IsNullOrEmpty(orderCreate.Address.Latitude))
                        orderCreate.Address.Latitude = orderCreate.Address.Latitude.Trim();

                    if (!string.IsNullOrEmpty(orderCreate.Address.Longitude))
                        orderCreate.Address.Longitude = orderCreate.Address.Longitude.Trim();
                }
                if(!string.IsNullOrEmpty(orderCreate.Notes))
                    orderCreate.Notes = orderCreate.Notes.Trim();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
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

                if (request.SourceId < 1)
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

        private void ThrowError(string message, ErrorNumber errorNumber)
        {
            try
            {
                throw new RestaurantException
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = message,
                        ErrorNumber = errorNumber
                    }
                };
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void ValidateNames(string name, string nameAr)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Name cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (string.IsNullOrEmpty(nameAr))
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

    }
}
