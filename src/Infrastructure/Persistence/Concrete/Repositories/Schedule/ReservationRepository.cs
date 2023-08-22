namespace Space.Infrastructure.Persistence;


internal class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
    
    public ReservationRepository(SpaceDbContext context) : base(context) { }

    }



