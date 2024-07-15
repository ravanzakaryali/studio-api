namespace Space.Application.Handlers;

public class CreateUserCommand : IRequest
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
internal class CreateUserHandler : IRequestHandler<CreateUserCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly UserManager<User> _userManager;

    public CreateUserHandler(ISpaceDbContext spaceDbContext, UserManager<User> userManager)
    {
        _spaceDbContext = spaceDbContext;
        _userManager = userManager;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null) throw new AlreadyExistsException(nameof(User), request.Email);

        IdentityResult result = await _userManager.CreateAsync(new Worker
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            UserName = request.Email
        }, request.Password);

        if (!result.Succeeded) throw new Exception("Create user exception");
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}