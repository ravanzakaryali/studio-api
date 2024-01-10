namespace Space.Application.Handlers.Validations;

public class RefreshPasswordCommandValidation : AbstractValidator<RefreshPasswordCommand>
{
    public RefreshPasswordCommandValidation()
    {

        RuleFor(rpc=> rpc.Email)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .EmailAddress().WithMessage(Constants.ValidationEmailMessage)
            .Matches(Constants.EmailRegex);

    }
}
