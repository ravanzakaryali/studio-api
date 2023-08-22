namespace Space.Application.Handlers.Validations;

public class UpdateWorkerCommandValidation : AbstractValidator<UpdateWorkerCommand>
{
    public UpdateWorkerCommandValidation()
    {
        RuleFor(i => i.Worker.Name)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(i => i.Worker.Surname)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(i=>i.Worker.Email)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .Matches(Constants.EmailRegex).WithMessage(Constants.ValidationEmailMessage);
    }
}
