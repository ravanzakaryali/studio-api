using System;
using Space.Application.DTOs.Class.Response;

namespace Space.Application.Handlers
{

    public record GetClassByProgramQuery(Guid id) : IRequest<IEnumerable<GetClassByProgramQueryDto>>;

    internal class GetClassByProgramQueryHandler : IRequestHandler<GetClassByProgramQuery, IEnumerable<GetClassByProgramQueryDto>>
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IClassRepository _classRepository;

        public GetClassByProgramQueryHandler(
            IUnitOfWork unitOfWork,
            IClassRepository classRepository)
        {
            _unitOfWork = unitOfWork;
            _classRepository = classRepository;
        }


        public async Task<IEnumerable<GetClassByProgramQueryDto>> Handle(GetClassByProgramQuery request, CancellationToken cancellationToken)
        {
            var classes = await _classRepository.GetAllAsync(q => q.ProgramId == request.id) ??
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

