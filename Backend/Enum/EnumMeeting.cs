using ST.INFRA;

namespace Backend.Enum
{
    public class EnumMeeting
    {
        #region Enum
        public enum ActionId
        {
            InProcess = 10,
            Booking = 11,
            Cancel = 12,
        }

        public enum ActionDate
        {
            lastUpdate = 1,
            BookingDate = 2,
        }
         public enum ActionType
        {
            Co = 5,
            Hr = 9
        }
        #endregion

    }
}

