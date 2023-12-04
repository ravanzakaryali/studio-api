namespace Space.Application.Handlers;

public class UpdateClassSessionAtttendanceCommandValidation : AbstractValidator<CreateClassSessionAttendanceCommand>
{
    public UpdateClassSessionAtttendanceCommandValidation()
    {
        RuleFor(c => c.Date)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        
        RuleFor(c => c.ClassId)
             .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleForEach(c => c.Sessions).SetValidator(new UpdateAttendanceCategorySessionDtoValidation());
    }
}
public class UpdateAttendanceCategorySessionDtoValidation : AbstractValidator<UpdateAttendanceCategorySessionDto>
{
    public UpdateAttendanceCategorySessionDtoValidation()
    {
        //RuleFor(c => c.WorkerId)
        //    .NotNull().WithMessage(Constants.ValidationRequiredMessage)
        //    .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
        RuleFor(c => c.Category)
            .IsInEnum();
        RuleFor(c => c.Status)
            .IsInEnum();
        RuleForEach(c => c.Attendances).SetValidator(new UpdateAttendanceDtoValidation());
    }
}
public class UpdateAttendanceDtoValidation : AbstractValidator<UpdateAttendanceDto>
{
    public UpdateAttendanceDtoValidation()
    {
        RuleFor(c=>c.StudentId)
            .NotNull().WithMessage(Constants.ValidationRequiredMessage)
            .NotEmpty().WithMessage(Constants.ValidationRequiredMessage);
    }
}