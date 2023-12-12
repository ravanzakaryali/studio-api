using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.Handlers
{

    public record GetWorkerAttendanceQuery(int Id, DateTime date) : IRequest<IEnumerable<GetWorkerAttendanceDto>>;

    internal class GetWorkerAttendanceQueryHandler : IRequestHandler<GetWorkerAttendanceQuery, IEnumerable<GetWorkerAttendanceDto>>
    {

        readonly IUnitOfWork _unitOfWork;

        public GetWorkerAttendanceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
       
        }

        public Task<IEnumerable<GetWorkerAttendanceDto>> Handle(GetWorkerAttendanceQuery request, CancellationToken cancellationToken)
        {

           //
            throw new NotImplementedException();
        }
    }
}
