using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class BaseController : ApiController
    {
        #region Branch_Helpers
        protected void ValidateCreateBranch(Branch branch)
        {
            try
            {
                if (string.IsNullOrEmpty(branch.Name))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Name cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
                
                if (string.IsNullOrEmpty(branch.Name))
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
        #endregion

        #region MealType_Helpers
        protected void ValidateCreateMealType(MealType mealType)
        {
            try
            {
                if (string.IsNullOrEmpty(mealType.Name))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Name cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (string.IsNullOrEmpty(mealType.Name))
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
        #endregion

        #region Shared
        protected void TrimNames(BasicEntity entity)
        {
            entity.Name = entity.Name.Trim();
            entity.NameAr = entity.NameAr.Trim();
        }
        #endregion
    }
}
