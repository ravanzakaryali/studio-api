using Space.Application.Handlers;

namespace Space.Application.Handlers;

public class ConfirmCodeCommandValidation : AbstractValidator<ConfirmCodeCommand>
{
    public ConfirmCodeCommandValidation()
    {
        RuleFor(a => a.Email)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .EmailAddress().WithMessage(Constants.ValidationEmailMessage)
            .Matches(@"^[a-zA-Z0-9._%+-]+@code.edu.az$\").WithMessage(Constants.EmailRegex);
        RuleFor(a => a.Code)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

    }
}
