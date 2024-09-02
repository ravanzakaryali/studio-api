namespace Space.Application.Handlers.Validations;

public class CreateClassCommandValidators : AbstractValidator<CreateClassCommand> 
{
	public CreateClassCommandValidators()
	{
		RuleFor(c => c.ProgramId)
			.NotNull().WithMessage(Constants.ValidationRequiredMessage)
			.NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c => c.SessionId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
    }
}
