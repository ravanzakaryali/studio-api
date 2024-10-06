using System;

namespace Space.Application.Handlers
{
	public record CreateExamSheetCommand(IEnumerable<CreateExamSheetDto> CreateExamSheetDtos) : IRequest<CreateExamSheetResponse>;
	public class CreateExamSheetResponse{}

    public class CreateExamSheetHadnler : IRequestHandler<CreateExamSheetCommand, CreateExamSheetResponse>
    {
        private readonly ISpaceDbContext _dbContext;

        public CreateExamSheetHadnler(ISpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateExamSheetResponse> Handle(CreateExamSheetCommand request, CancellationToken cancellationToken)
        {


            List<ExamSheet> examSheets = request.CreateExamSheetDtos.Select(es => new ExamSheet { ModuleId = es.ModuleId, ClassId = es.ClassId }).ToList();

            List<ExamSheet> dbExamSherts = await _dbContext.ExamSheets.ToListAsync();

            if (!request.CreateExamSheetDtos.Any())
            {
                _dbContext.ExamSheets.RemoveRange();
            }

            foreach (var item in request.CreateExamSheetDtos)
            {
                var dbItem = dbExamSherts.FirstOrDefault(c => c.ModuleId == item.ModuleId && c.ClassId == item.ClassId);
                if (dbItem == null)
                {
                    _dbContext.ExamSheets.Add(new ExamSheet()
                    {
                        ModuleId = item.ModuleId,
                        ClassId = item.ClassId
                    });
                }
                else
                {
                    _dbContext.ExamSheets.Remove(dbItem);
                };
            }

            await _dbContext.SaveChangesAsync();

            return new CreateExamSheetResponse();
        }
    }


}

