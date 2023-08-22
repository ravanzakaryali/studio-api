namespace Space.Application.Common;

internal static class Constants
{
    public const string ValidationRequiredMessage = "The '{PropertyName}' field is required.",
                        ValidationEmailMessage = "The end of the '{PropertyName}' should end with @code.edu.az",
                        ValidationMaxLengthMessage = "The '{PropertyName}' field must be at most '{MaxLength}' characters long.",
                        ValidationMinLengthMessage = "The '{PropertyName}' field minumum must be at most '{MinLength}' characters long.",
                        EmailRegex = @"^[a-zA-Z0-9._%+-]+@code.edu.az$";
}
