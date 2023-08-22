namespace Space.Application.Handlers.Validations;

internal class CreateSessionDetailCommandValidation : AbstractValidator<CreateSessionDetailCommand>
{
    public CreateSessionDetailCommandValidation()
    {
        RuleFor(a => a.Id)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Details.StartTime)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Details.EndTime)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Details.DayOfWeek)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage).IsInEnum();
    }
}
