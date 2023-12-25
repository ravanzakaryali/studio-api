using ClosedXML.Excel;

namespace Space.Application.Handlers;

public class StudentsOfClassesExcelExportCommand : IRequest<FileContentResponseDto>
{
    public ClassStatus ClassStatus { get; set; }
    public List<int>? ClassIds { get; set; }
    public List<int>? ProgramIds { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

internal class StudentsOfClassesExcelExportHandler
    : IRequestHandler<StudentsOfClassesExcelExportCommand, FileContentResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IHttpContextAccessor _contextAccessor;

    public StudentsOfClassesExcelExportHandler(
        ISpaceDbContext spaceDbContext,
        IHttpContextAccessor contextAccessor
    )
    {
        _spaceDbContext = spaceDbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task<FileContentResponseDto> Handle(
        StudentsOfClassesExcelExportCommand request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Class> query = _spaceDbContext
            .Classes
            .Include(c => c.ClassTimeSheets)
            .ThenInclude(c => c.Attendances)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c!.Contact)
            .AsQueryable();

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        if (request.ClassStatus == ClassStatus.Active)
        {
            query = query.Where(c => dateNow > c.StartDate && dateNow < c.EndDate);
        }
        else if (request.ClassStatus == ClassStatus.Close)
        {
            query = query.Where(c => dateNow > c.EndDate);
        }
        else if (request.ClassStatus == ClassStatus.New)
        {
            query = query.Where(c => dateNow < c.StartDate && dateNow < c.EndDate);
        }

        if (request.ClassIds != null && request.ClassIds.Count > 0)
        {
            query = query.Where(c => request.ClassIds.Contains(c.Id));
        }
        if (request.ProgramIds != null && request.ProgramIds.Count > 0)
        {
            query = query.Where(c => request.ProgramIds.Contains(c.ProgramId));
        }
        if (request.StartDate != null)
        {
            DateOnly startDate = DateOnly.FromDateTime(request.StartDate.Value);
            query = query.Where(c => c.StartDate >= startDate);
        }
        if (request.EndDate != null)
        {
            DateOnly endDate = DateOnly.FromDateTime(request.EndDate.Value);
            query = query.Where(c => c.EndDate <= endDate);
        }
        List<Class> classes = await query.ToListAsync(cancellationToken);

        List<StudentExcelExportDto> allStudents = classes
                .SelectMany(currentClass => currentClass.ClassTimeSheets
                    .SelectMany(c => c.Attendances
                        .Select(at => new StudentExcelExportDto()
                        {
                            ClassName = currentClass.Name,
                            Date = c.Date,
                            Email = at.Student.Student!.Email,
                            Name = at.Student.Student.Contact!.Name,
                            Surname = at.Student.Student.Contact.Surname,
                            TotalHour = at.TotalAttendanceHours
                        })))
                .ToList();

        using XLWorkbook workbook = new();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Davamiyyət göstəriciləri");

        worksheet.Cell(1, 1).Value = "Grup No";
        worksheet.Cell(1, 2).Value = "Adı";
        worksheet.Cell(1, 3).Value = "Soyadı";
        worksheet.Cell(1, 4).Value = "Email";
        worksheet.Cell(1, 5).Value = "Tarix";
        worksheet.Cell(1, 6).Value = "İştirak saatı";

        worksheet.Column(2).Width = 10;
        worksheet.Column(3).Width = 15;
        worksheet.Column(4).Width = 30;
        worksheet.Column(5).Width = 15;
        worksheet.Column(6).Width = 10;

        worksheet.Cell(2, 1).InsertData(allStudents.OrderBy(c => c.Date));

        MemoryStream memoryStream = new();
        workbook.SaveAs(memoryStream);
        byte[] excelData = memoryStream.ToArray();

        if (_contextAccessor.HttpContext is null) throw new Exception("Http context is null");

        _contextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=All_Classes_{DateTime.Now:yyyy-MM-dd}.xlsx");
        _contextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        await _contextAccessor.HttpContext.Response.Body.WriteAsync(excelData);
        await _contextAccessor.HttpContext.Response.CompleteAsync();

        FileContentResponseDto file = new()
        {
            Name = $"All_Classes_{DateTime.Now:yyyy-MM-dd}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileBytes = excelData
        };

        return file;
    }
}
