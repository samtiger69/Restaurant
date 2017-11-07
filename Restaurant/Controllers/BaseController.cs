﻿using Restaurant.Entities;
using Restaurant.Models;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class BaseController : ApiController
    {
        #region Shared
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
        #endregion

        #region Order_Helpers
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
                orderCreate.Notes = orderCreate.Notes.Trim();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
