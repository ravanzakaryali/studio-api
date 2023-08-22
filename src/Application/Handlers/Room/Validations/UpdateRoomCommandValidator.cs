namespace Space.Application.Handlers.Validations;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
	public UpdateRoomCommandValidator()
	{
        RuleFor(a => a.UpdateRoom.Name)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.UpdateRoom.Capacity)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(a => a.UpdateRoom.Type)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .IsInEnum();
    }
}
