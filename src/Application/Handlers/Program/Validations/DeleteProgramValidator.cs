namespace Space.Application.Handlers.Validations;

public class DeleteProgramValidator : AbstractValidator<DeleteProgramCommand>
{
    public DeleteProgramValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
    }
}
