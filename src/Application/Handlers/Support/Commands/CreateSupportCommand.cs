using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateSupportCommand : IRequest
{
    public CreateSupportCommand(string title, string? description, int? classId, int categoryId, string phoneNumber, IFormFileCollection? images)
    {
        Title = title;
        Description = description;
        ClassId = classId;
        CategoryId = categoryId;
        PhoneNumber = phoneNumber;
        Images = images;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public int? ClassId { get; set; }
    public int CategoryId { get; set; }
    public string PhoneNumber { get; set; }
    public IFormFileCollection? Images { get; set; }

}
internal class CreateSupportCommandHandler : IRequestHandler<CreateSupportCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IStorageService _storageService;
    readonly ICurrentUserService _currentUserService;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateSupportCommandHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        ICurrentUserService currentUserService,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _currentUserService = currentUserService;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateSupportCommand request, CancellationToken cancellationToken)
    {
        string loginUserId = _currentUserService?.UserId
            ?? throw new UnauthorizedAccessException();

        User? user = await _unitOfWork.UserService.FindById(int.Parse(loginUserId))
            ?? throw new NotFoundException(nameof(User), "");

        user.PhoneNumber = request.PhoneNumber;

        SupportCategory? supportCategory = await _spaceDbContext.SupportCategories.FindAsync(request.CategoryId)
            ?? throw new NotFoundException(nameof(SupportCategory), request.CategoryId);

        Support newSupport = new()
        {
            UserId = user.Id,
            Title = request.Title,
            Description = request.Description,
            SupportCategoryId = supportCategory.Id,
        };

        if (request.ClassId != null)
        {
            Class? supportClass = await _spaceDbContext.Classes.FindAsync(request.ClassId) ?? throw new NotFoundException(nameof(Class), request.ClassId);
            newSupport.ClassId = supportClass.Id;
            newSupport.Class = supportClass;
        }

        if (request.Images != null && request.Images?.Count > 0)
        {
            List<FileUploadResponse> imageUpload = await _storageService.UploadAsync(request.Images, "Images");
            newSupport.SupportImages = imageUpload.Select(i => new SupportImage()
            {
                FileName = i.FileName,
                Path = i.PathName,
                Extension = i.Extension,
                Storage = "Local",
                Size = i.Size
            }).ToList();
        }

        await _spaceDbContext.Supports.AddAsync(newSupport, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);

        SendEmailSupportMessageDto sendEmailSupportMessageDto = new()
        {
            Message = request.Description,
            Title = request.Title,
            User = new UserDto()
            {
                Id = user.Id,
                Name = user.Name!,
                Surname = user.Surname!,
                Email = user.Email
            }
        };

        if (supportCategory.Redirect == SupportRedirect.Academic)
        {
            sendEmailSupportMessageDto.To = new List<string>(){
                "farhad.pashayev@code.edu.az"
            };
            await _unitOfWork.EmailService.SendSupportMessageAsync(sendEmailSupportMessageDto, newSupport.Id);

        }
        else if (supportCategory.Redirect == SupportRedirect.DigitalLab)
        {
            sendEmailSupportMessageDto.To = new List<string>(){
                "farhadip@code.edu.az"
            };
            await _unitOfWork.EmailService.SendSupportMessageAsync(sendEmailSupportMessageDto, newSupport.Id);
        }
        else if (supportCategory.Redirect == SupportRedirect.DigitalLabAndAcademic)
        {
            sendEmailSupportMessageDto.To = new List<string>(){
                "farhad.pashayev@code.edu.az",
                "farhadip@code.edu.az"
            };
            await _unitOfWork.EmailService.SendSupportMessageAsync(sendEmailSupportMessageDto, newSupport.Id);
        }


    }
}
