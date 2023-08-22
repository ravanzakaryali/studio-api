using System;
using Space.Application.DTOs.Class.Response;

namespace Space.Application.Handlers
{
    
    public record GetClassByProgramQuery(Guid id) : IRequest<IEnumerable<GetClassByProgramQueryDto>>;

    internal class GetClassByProgramQueryHandler : IRequestHandler<GetClassByProgramQuery, IEnumerable<GetClassByProgramQueryDto>>
    {

        readonly IUnitOfWork _unitOfWork;

        public GetClassByProgramQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<GetClassByProgramQueryDto>> Handle(GetClassByProgramQuery request, CancellationToken cancellationToken)
        {
            var classes = await _unitOfWork.ClassRepository.GetAllAsync(q => q.ProgramId == request.id) ??
            throw new NotFoundException(nameof(Class), request.id); ;

            var response = classes.Select(q => new GetClassByProgramQueryDto()
            {
                Id = q.Id,
                Name = q.Name
            });

            return response;
        }
    }

}

