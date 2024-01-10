namespace Space.Application.Handlers.Validations;

public class DeleteWorkerCommandValidation : AbstractValidator<DeleteWorkerCommand>
{
    public DeleteWorkerCommandValidation()
    {
        RuleFor(i=>i.Id)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
    }
}
