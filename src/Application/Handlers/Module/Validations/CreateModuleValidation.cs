namespace Space.Application.Handlers.Validations;

public class CreateModuleValidation : AbstractValidator<CreateModuleWithProgramCommand>
{
    public CreateModuleValidation()
    {
        RuleFor(m => m.ProgramId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleForEach(m => m.Modules).SetValidator(new CreateModuleDtoValidation());
    }
}   
public class CreateModuleDtoValidation : AbstractValidator<ModuleDto>
{
    public CreateModuleDtoValidation()
    {
        RuleFor(m => m.Name)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleForEach(m => m.SubModules).SetValidator(new CreateSubModuleDtoValidation());
    }
}
public class CreateSubModuleDtoValidation : AbstractValidator<CreateSubModuleDto>
{
    public CreateSubModuleDtoValidation()
    {
        RuleFor(m => m.Name)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(m => m.Hours).GreaterThan(0);
    }
}