namespace Restaurant.Models
{
    public class ErrorCode
    {
        public ErrorCode()
        {
        }

        public ErrorCode(string errorMessage, ErrorNumber errorNumber)
        {
            ErrorNumber = errorNumber;
            ErrorMessage = errorMessage;
        }

        public ErrorNumber ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ErrorNumber
    {
        Success = 0,

        NotFound = -1,

        Exists = -2,

        BranchDoesNotExist = -3,

        UserDoesNotExist = -4,

        MealDoesNotExist = -5,

        OrderDoesNotExist = -6,

        AttributeDoesNotExist = -7,

        OrderMealDoesNotExist = -8,

        DeliveryUserNotInOrderBranch = -9,

        UserNotBranchAdmin = -10,

        AttributeGroupDoesNotExist = -11,

        AccessDenied = -100,

        GeneralError = -200,

        EmptyRequiredField = -300,

    }
}