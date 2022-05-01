using HotelBooking.DataAccess.EF.Repositories;
using HotelBooking.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IGenericRepository<ReservationDate> _reservationDateRepository;
        private readonly IGenericRepository<Reservation> _reservationRepository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IRepositoryFactory repositoryFactory,
            ILogger<ReservationService> logger)
        {
            _reservationDateRepository = repositoryFactory.GetRepository<ReservationDate>();
            _reservationRepository = repositoryFactory.GetRepository<Reservation>();
            _logger = logger;
        }

        public async Task<List<DateTime>> GetListOfBookedDatesAsync(int roomtId)
        {
            var reservationDateList = await _reservationDateRepository.GetAsync(x => x.RoomId == roomtId);
            return reservationDateList.Select(x => x.Date).ToList();
        }

        public async Task SaveReservationAsync(Reservation reservation)
        {
            try
            {
                await _reservationRepository.AddAsync(reservation);
                await _reservationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Saving Reservation");
                throw;
            }
        }

        public async Task<List<Reservation>> GetReservationByUserIdAsync(int userId)
        {
            return await _reservationRepository.GetAsync(x => x.UserId == userId, new List<string>() { "ReservationDate" });
        }

        public async Task<Reservation> GetReservationAsync(int userId, Guid reservationGuid)
        {
            return await _reservationRepository.SingleOrDefaultAsync(x =>
                (x.UserId == userId && x.ReservationGuid == reservationGuid), new List<string>() { "ReservationDate" });
        }

        public async Task RemoveReservationAsync(Reservation reservation)
        {
            try
            {
                foreach (var reservationDate in reservation.ReservationDate)
                {
                    _reservationDateRepository.Remove(reservationDate);
                }
                _reservationRepository.Remove(reservation);
                await _reservationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Removing Reservation");
                throw;
            }

        }

        public async Task UpdateReservationDateAsync(IEnumerable<ReservationDate> reservationDatesToRemove,
            IEnumerable<ReservationDate> reservationDatesToAdd)
        {
            try
            {
                _reservationDateRepository.RemoveRange(reservationDatesToRemove.ToArray());
                await _reservationDateRepository.AddRangeAsync(reservationDatesToAdd.ToArray());
                await _reservationDateRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Updating ReservationDates");
                throw;
            }
        }

        public int GetReservationRoomId(Reservation reservation)
        {
            return reservation?.ReservationDate?.FirstOrDefault()?.RoomId ?? 0;
        }

    }
}
