using System;
namespace Space.Application.Handlers
{
    public record CreateSurveySheetCommand(IEnumerable<CreateSurveySheetDto> CreateSurveySheetDtos) : IRequest<CreateSurveySheetResponse>;
    public class CreateSurveySheetResponse { }
    public class CreateSurveySheetHadnler : IRequestHandler<CreateSurveySheetCommand, CreateSurveySheetResponse>
    {
        private readonly ISpaceDbContext _dbContext;

        public CreateSurveySheetHadnler(ISpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateSurveySheetResponse> Handle(CreateSurveySheetCommand request, CancellationToken cancellationToken)
        {


            List<SurveySheet> surveySheets = request.CreateSurveySheetDtos.Select(es => new SurveySheet { ModuleId = es.ModuleId, ClassId = es.ClassId }).ToList();

            List<SurveySheet> dbSurveySherts = await _dbContext.SurveySheets.ToListAsync();

            if (!request.CreateSurveySheetDtos.Any())
            {
                _dbContext.SurveySheets.RemoveRange();
            }

            foreach (var item in request.CreateSurveySheetDtos)
            {
                var dbItem = dbSurveySherts.FirstOrDefault(c => c.ModuleId == item.ModuleId && c.ClassId == item.ClassId);
                if (dbItem == null)
                {
                    _dbContext.SurveySheets.Add(new SurveySheet()
                    {
                        ModuleId = item.ModuleId,
                        ClassId = item.ClassId
                    });
                }
                else
                {
                    _dbContext.SurveySheets.Remove(dbItem);
                };
            }

            await _dbContext.SaveChangesAsync();

            return new CreateSurveySheetResponse();
        }
    }

}

