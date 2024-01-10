namespace Space.Application.Handlers.Validations;

public class CreateProgramValidator : AbstractValidator<CreateProgramCommand>
{
	public CreateProgramValidator()
	{
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
    }
}
