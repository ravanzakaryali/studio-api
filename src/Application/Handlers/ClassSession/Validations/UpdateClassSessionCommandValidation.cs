namespace Space.Application.Handlers;

public class UpdateClassSessionCommandValidation : AbstractValidator<UpdateClassSessionCommand>
{
    public UpdateClassSessionCommandValidation()
    {
        RuleFor(c=>c.Id)
             .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c=>c.Date)
             .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleForEach(c => c.UpdateClassSessions).SetValidator(new UpdateClassSessionRequestDtoValidation());
    }
}
public class UpdateClassSessionRequestDtoValidation : AbstractValidator<UpdateClassSessionRequestDto>
{
    public UpdateClassSessionRequestDtoValidation()
    {
        RuleFor(c=>c.Category)
            .IsInEnum()
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c=>c.EndTime)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c=>c.StartTime)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c=>c.ClassSessionDate)
              .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
    }
}