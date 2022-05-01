using HotelBooking.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<DateTime>> GetListOfBookedDatesAsync(int roomId);
        Task SaveReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationByUserIdAsync(int userId);
        Task<Reservation> GetReservationAsync(int userId, Guid reservationGuid);
    }
}
