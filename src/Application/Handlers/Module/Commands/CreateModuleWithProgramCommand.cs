using Space.Domain.Entities;
using System.Collections;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Space.Application.Exceptions;

namespace Space.Application.Handlers.Commands;

public record CreateModuleWithProgramCommand(int ProgramId, IEnumerable<ModuleDto> Modules) : IRequest<IEnumerable<GetModuleDto>>;

internal class CreateModuleCommandHandler : IRequestHandler<CreateModuleWithProgramCommand, IEnumerable<GetModuleDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateModuleCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(CreateModuleWithProgramCommand request, CancellationToken cancellationToken)
    {
        List<Module> modules = _mapper.Map<List<Module>>(request.Modules)
            .Select(m =>
            {
                m.ProgramId = request.ProgramId;
                if (m.SubModules?.Count != 0)
                {
                    m.Hours = m.SubModules!.Sum(subModule => subModule.Hours);
                    m.SubModules = m.SubModules!.Select(sm =>
                    {
                        sm.ProgramId = request.ProgramId;
                        return sm;
                    }).ToList();
                    return m;
                }
                else
                {
                    return m;
                }
            }).ToList();

        Program? program = await _spaceDbContext.Programs
            .Include(c => c.Modules)
            .Where(a => a.Id == request.ProgramId)
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(nameof(Program), request.ProgramId);

        if (program == null)
        {
            throw new NotFoundException(nameof(Program), request.ProgramId);
        }

        List<Module> existingModules = await _spaceDbContext.Modules
            .Where(m => m.ProgramId == request.ProgramId)
            .ToListAsync();

        List<Module> modulesToUpdate = modules.Where(m => existingModules.Any(em => em.Id == m.Id)).ToList();
        List<Module> modulesToAdd = modules.Where(m => !existingModules.Any(em => em.Id == m.Id)).ToList();
        List<Module> modulesToDelete = existingModules.Where(em => !modules.Any(m => m.Id == em.Id)).ToList();

        foreach (Module? module in modulesToUpdate)
        {
            Module existingModule = existingModules.First(em => em.Id == module.Id);
            existingModule.Name = module.Name;
            existingModule.Hours = module.Hours;
            existingModule.SubModules = module.SubModules;
        }

        if (modulesToAdd.Any())
        {
            await _spaceDbContext.Modules.AddRangeAsync(modulesToAdd);
        }

        if (modulesToDelete.Any())
        {
            _spaceDbContext.Modules.RemoveRange(modulesToDelete);
        }

        if (program.TotalHours != modules.Sum(m => m.Hours))
        {
            IEnumerable<ValidationFailure> failure = new List<ValidationFailure>()
            {
                new ValidationFailure("TotalHours",$"The total {program.TotalHours} hours of the '{nameof(Program)}' must be equal to the sum of the hours of the '{nameof(Program)}'")
            };
            throw new ValidationException("Program hours error", failure);
        }

        await _spaceDbContext.SaveChangesAsync();

        return _mapper.Map<IEnumerable<GetModuleDto>>(modules);
    }
}
