namespace Space.Application.Handlers.Validations;

public class CreateWorkerCommanValidation : AbstractValidator<CreateWorkerCommand>
{
    public CreateWorkerCommanValidation()
    {
        RuleFor(a => a.Email)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .EmailAddress().WithMessage(Constants.ValidationEmailMessage)
            .Matches(Constants.EmailRegex).WithMessage(Constants.ValidationEmailMessage);
        RuleFor(a => a.Surname)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
    }
}
