namespace Space.Application.Handlers.Validations;

public class CreateClassModuleCommandValidatiors : AbstractValidator<CreateClassModuleCommand>
{
    public CreateClassModuleCommandValidatiors()
    {
        RuleFor(c => c.ClassId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

        RuleForEach(c => c.CreateClassModule).SetValidator(new CreateClassModuleRequestDtoValidation());
    }
}
public class CreateClassModuleRequestDtoValidation : AbstractValidator<CreateClassModuleRequestDto>
{
    public CreateClassModuleRequestDtoValidation()
    {
        RuleFor(c => c.EndDate)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(c => c.StartDate)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(c => c.ModuleId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(c => c.RoleId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

        RuleFor(c => c.WorkerId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);

    }
}
