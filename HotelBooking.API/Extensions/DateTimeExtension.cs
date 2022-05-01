using System;
using HotelBooking.DataAccess.EF.Constants;

namespace HotelBooking.API.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToStringDefault(this DateTime date)
        {
            return date.ToString(Constants.DEFAULT_DATE_FORMAT);
        }
    }
}
