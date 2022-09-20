using Application.Exceptions;

namespace Application.Helpers
{
    public static class DateTimeHelper
    {
        public const int OneDayInUNIX = 86400;

        public static int GetNumberOfDaysInUNIX(int numberOfDays)
        {
            if (numberOfDays <= 0)
                throw new ApiException();

            return numberOfDays * OneDayInUNIX;
        }

        public static int GetUNIXInNumberOfDays(int daysInUNIX)
        {
            if (daysInUNIX % OneDayInUNIX != 0 || daysInUNIX <= 0)
                throw new ApiException();

            return daysInUNIX / OneDayInUNIX;
        }
    }
}
