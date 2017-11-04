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

        GeneralError = 100,

        EmptyRequiredField = 1,

    }
}