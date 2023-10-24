using Space.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.Handlers
{
    public record GetClassSessionsByDateQuery(Guid id, DateTime date) : IRequest<IEnumerable<GetClassSessionsByDateResponseDto>>;


    internal class GetClassSessionsByDateQueryHandler : IRequestHandler<GetClassSessionsByDateQuery, IEnumerable<GetClassSessionsByDateResponseDto>>
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IClassSessionRepository _classSessionRepository;

        public GetClassSessionsByDateQueryHandler(
            IUnitOfWork unitOfWork, 
            IClassSessionRepository classSessionRepository)
        {
            _unitOfWork = unitOfWork;
            _classSessionRepository = classSessionRepository;
        }

        public async Task<IEnumerable<GetClassSessionsByDateResponseDto>> Handle(GetClassSessionsByDateQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ClassSession> classSessions = await _classSessionRepository.GetAllAsync(c => c.Date.Year == request.date.Year && c.Date.Day == request.date.Day && c.Date.Month == request.date.Month && c.ClassId == request.id);

            IEnumerable<GetClassSessionsByDateResponseDto> response = classSessions.Select(q => new GetClassSessionsByDateResponseDto()
            {
                StartTime = q.StartTime,
                EndTime = q.EndTime,
                Status = q.Status,
                TotalHour = q.TotalHour,
                Id = q.Id,
            });

            return response;
        }
    }



}
