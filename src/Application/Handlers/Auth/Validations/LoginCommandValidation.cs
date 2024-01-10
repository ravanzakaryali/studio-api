namespace Space.Application.Handlers;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
	public LoginCommandValidation()
	{
		RuleFor(a => a.Email)
			.NotNull().WithMessage(Constants.ValidationRequiredMessage)
			.NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
			.EmailAddress().WithMessage(Constants.ValidationEmailMessage)
			.Matches(@"^[a-zA-Z0-9._%+-]+@code.edu.az$").WithMessage(Constants.ValidationEmailMessage);
		RuleFor(a => a.Password)
			.NotEmpty().WithMessage(Constants.ValidationRequiredMessage)
			.NotNull().WithMessage(Constants.ValidationRequiredMessage);
	}
}
