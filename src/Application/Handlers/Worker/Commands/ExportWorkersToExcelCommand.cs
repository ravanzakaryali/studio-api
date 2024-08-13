using ClosedXML.Excel;

public class ExportWorkersToExcelCommand : IRequest<FileContentResponseDto>
{
    public WorkerType? WorkerType { get; set; }
}
internal class ExportWorkersToExcelHandler : IRequestHandler<ExportWorkersToExcelCommand, FileContentResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IHttpContextAccessor _httpContextAccessor;

    public ExportWorkersToExcelHandler(
        ISpaceDbContext spaceDbContext,
        IHttpContextAccessor contextAccessor
    )
    {
        _spaceDbContext = spaceDbContext;
        _httpContextAccessor = contextAccessor;
    }

    public async Task<FileContentResponseDto> Handle(
        ExportWorkersToExcelCommand request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Class> query = _spaceDbContext.Classes
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .AsQueryable();

        if (request.WorkerType != null)
        {
            query = query.Where(c => c.ClassModulesWorkers.Any(c => c.WorkerType == request.WorkerType));
        }
        List<Class> classes = await query.ToListAsync();

        using XLWorkbook workbook = new XLWorkbook();

        IXLWorksheet worksheet = workbook.Worksheets.Add("Akademik heyət");

        worksheet.Cell(1, 1).Value = "Adı";
        worksheet.Cell(1, 2).Value = "Soyadı";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Qrup adı";

        worksheet.Cell(2, 1).InsertData(classes.SelectMany(c => c.ClassModulesWorkers.Select(c => new
        {
            c.Worker.Name,
            c.Worker.Surname,
            c.Worker.Email,
            ClassName = c.Class.Name
        })).DistinctBy(c => c.Email));

        using MemoryStream memoryStream = new();
        workbook.SaveAs(memoryStream);
        byte[] excelData = memoryStream.ToArray();


        if (_httpContextAccessor.HttpContext is null) throw new Exception("Http context is null");
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=Aktiv_Heyat_{DateTime.Now:yyyy-mm-dd}.xlsx");
        _httpContextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        await _httpContextAccessor.HttpContext.Response.Body.WriteAsync(excelData);
        await _httpContextAccessor.HttpContext.Response.CompleteAsync();

        return new FileContentResponseDto()
        {
            Name = $"Aktiv_Heyat_{DateTime.Now:yyyy-mm-dd}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileBytes = excelData
        };
    }

}