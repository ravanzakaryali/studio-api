using ClosedXML.Excel;

namespace Space.Application.Handlers;


public class StudentsofClassExcelExportCommand : IRequest<FileContentResponseDto>
{
    public Guid ClassId { get; set; }
}
internal class StudentsofClassExcelExport : IRequestHandler<StudentsofClassExcelExportCommand, FileContentResponseDto>
{
    readonly ISpaceDbContext _dbContext;
    readonly IHttpContextAccessor _httpContextAccessor;

    public StudentsofClassExcelExport(
        ISpaceDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    //Todo: Excel export update - add column Fin code,Class name
    public async Task<FileContentResponseDto> Handle(StudentsofClassExcelExportCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _dbContext.Classes
                                                .Where(c => c.Id == request.ClassId)
                                                .Include(c => c.ClassSessions)
                                                .ThenInclude(c => c.Attendances)
                                                .ThenInclude(c => c.Student)
                                                .ThenInclude(c => c.Student)
                                                .ThenInclude(c => c!.Contact)
                                                .FirstOrDefaultAsync()
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        IEnumerable<StudentExcelExportDto> students = @class.ClassSessions.Where(c => c.Status != null).SelectMany(c =>
        {
            List<StudentExcelExportDto> studentResponse = new();
            studentResponse.AddRange(c.Attendances.Select(at => new StudentExcelExportDto()
            {
                ClassName = @class.Name,
                Date = c.Date,
                Email = at.Student.Student!.Contact?.Email,
                Name = at.Student.Student.Contact!.Name,
                Surname = at.Student.Student.Contact.Surname,
                TotalHour = at.TotalAttendanceHours
            }));
            return studentResponse;
        });
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Davamiyyət göstəriciləri");

        worksheet.Cell(1, 1).Value = "Adı";
        worksheet.Cell(1, 2).Value = "Soyadı";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Tarix";
        worksheet.Cell(1, 5).Value = "İştirak saatı";

        worksheet.Column(4).Width = 30;
        worksheet.Column(4).Width = 30;

        worksheet.Cell(2, 1).InsertData(students.OrderByDescending(c => c.Date));
        using MemoryStream memoryStream = new();
        workbook.SaveAs(memoryStream);
        byte[] excelData = memoryStream.ToArray();


        if (_httpContextAccessor.HttpContext is null) throw new Exception("Http context is null");
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={@class.Name}_{DateTime.Now:yyyy-mm-dd}.xlsx");
        _httpContextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        await _httpContextAccessor.HttpContext.Response.Body.WriteAsync(excelData);

        return new FileContentResponseDto()
        {
            Name = $"{@class.Name}_{DateTime.Now:yyyy-mm-dd}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileBytes = excelData
        };
    }
}
