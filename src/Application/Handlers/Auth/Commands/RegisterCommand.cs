namespace Space.Application.Handlers;

public class RegisterCommand : IRequest<RegisterResponseDto>
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{

    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        User user = await _unitOfWork.IdentityService.RegisterAsync(_mapper.Map<RegisterDto>(request));
        string code = _unitOfWork.TokenService.GenerateVerificationCode();
        user.ConfirmCode = code;
        user.ConfirmCodeExpires = DateTime.UtcNow.AddMinutes(15);
        await _unitOfWork.EmailService.SendMessageAsync(code, user.Email, "EmailTemplate.html");
        await _spaceDbContext.SaveChangesAsync();
        return new RegisterResponseDto()
        {
            Email = user.Email,
            Message = "Send email confirm code"
        };
    }

}
