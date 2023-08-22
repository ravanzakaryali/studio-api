namespace Space.Application.Handlers;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(a => a.Email)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .EmailAddress().WithMessage(Constants.ValidationEmailMessage)
            .Matches(Constants.EmailRegex).WithMessage(Constants.ValidationEmailMessage);
        RuleFor(a => a.Name)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .MinimumLength(5).WithMessage(Constants.ValidationMinLengthMessage);
        RuleFor(a => a.Surname)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .MinimumLength(5).WithMessage(Constants.ValidationMinLengthMessage);
        RuleFor(a => a.Password)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
    }
}
