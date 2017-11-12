using Restaurant.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class BaseService
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        protected T GetValue<T>(object readerValue, T defaultValue = default(T))
        {
            if (readerValue == null || readerValue == DBNull.Value)
                return defaultValue;
            else
                return (T)Convert.ChangeType(readerValue, typeof(T));
        }

        protected void ExecuteNonQuery(string storedProcedure, Action<SqlCommand> fillCommand)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected string ExecuteScalar(string storedProcedure, Action<SqlCommand> fillCommand)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    connection.Open();
                    return (command.ExecuteScalar()).ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void ExecuteReader(string storedProcedure, Action<SqlCommand> fillCommand, Action<SqlDataReader> fetchData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = storedProcedure,
                        Connection = connection
                    };
                    fillCommand(command);
                    connection.Open();
                    fetchData(command.ExecuteReader());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void HandleErrorCode(ErrorNumber errorNumber)
        {
            try
            {
                switch (errorNumber)
                {
                    case ErrorNumber.Success:
                        break;
                    case ErrorNumber.NotFound:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Object not found",
                                ErrorNumber = ErrorNumber.NotFound
                            }
                        };
                    case ErrorNumber.Exists:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "An object with the name/nameAr already exists in the db",
                                ErrorNumber = ErrorNumber.Exists
                            }
                        };
                    case ErrorNumber.BranchDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "branch does not exist",
                                ErrorNumber = ErrorNumber.BranchDoesNotExist
                            }
                        };
                    case ErrorNumber.UserDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "User does not exist",
                                ErrorNumber = ErrorNumber.UserDoesNotExist
                            }
                        };
                    case ErrorNumber.MealDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Meal does not exist",
                                ErrorNumber = ErrorNumber.MealDoesNotExist
                            }
                        };
                    case ErrorNumber.OrderDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Order does not exist",
                                ErrorNumber = ErrorNumber.OrderDoesNotExist
                            }
                        };
                    case ErrorNumber.AttributeDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "attribute does not exist",
                                ErrorNumber = ErrorNumber.AttributeDoesNotExist
                            }
                        };
                    case ErrorNumber.OrderMealDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "order meal does not exist",
                                ErrorNumber = ErrorNumber.OrderMealDoesNotExist
                            }
                        };
                    case ErrorNumber.DeliveryUserNotInOrderBranch:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Delivery man not in order branch",
                                ErrorNumber = ErrorNumber.DeliveryUserNotInOrderBranch
                            }
                        };
                    case ErrorNumber.UserNotBranchAdmin:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "User is not branch admin",
                                ErrorNumber = ErrorNumber.UserNotBranchAdmin
                            }
                        };
                    case ErrorNumber.AccessDenied:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Access Denied",
                                ErrorNumber = ErrorNumber.AccessDenied
                            }
                        };
                    case ErrorNumber.AttributeGroupDoesNotExist:
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "AttributeGroup Does Not Exist",
                                ErrorNumber = ErrorNumber.AttributeGroupDoesNotExist
                            }
                        };
                    case ErrorNumber.GeneralError:
                        break;
                    case ErrorNumber.EmptyRequiredField:
                        break;
                    default:
                        break;
                }
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }
    }
}