namespace Space.Application.Handlers.Validations;

public class CreateRoomCommandValidation : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidation()
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Capacity)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.Type)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .IsInEnum();
    }
}
