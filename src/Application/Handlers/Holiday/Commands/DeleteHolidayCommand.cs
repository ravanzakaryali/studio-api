namespace Space.Application.Handlers;

public record DeleteHolidayCommand(Guid Id) : IRequest<HolidayResponseDto>;

internal class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, HolidayResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteHolidayCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<HolidayResponseDto> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _spaceDbContext.Holidays.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);

        holiday.IsDeleted = true;
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
