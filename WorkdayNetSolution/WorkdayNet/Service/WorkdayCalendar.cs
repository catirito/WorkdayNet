using System;
using System.Collections.Generic;

namespace WorkdayNet.Service
{
    public class WorkdayCalendar : IWorkdayCalendar
    {

        private HashSet<DateTime> holidays;
        private HashSet<DateTime> weekendDays;

        private int startHours = 0, startMinutes = 0, stopHours = 23, stopMinutes = 59;
        private double totalTimeInWorkingDay = 0;

        public WorkdayCalendar()
        {
            holidays = new();
            weekendDays = new();

            SetWeekends();

        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {

            startDate = NormilizeStartDateForIncrement(startDate, incrementInWorkdays > 0);

            DateTime endDate = DateIncrement(startDate, incrementInWorkdays);

            endDate = AdaptIncrementToHolidaysAndWeekendDays(startDate, endDate);

            return endDate;
            
        }

        private DateTime AdaptIncrementToHolidaysAndWeekendDays(DateTime startDate, DateTime endDate)
        {
            
            List<DateTime> nonWorkingdays = new();

            nonWorkingdays.AddRange(holidays);
            nonWorkingdays.AddRange(weekendDays);

            int incrementValue = startDate < endDate ? 1 : -1;
            
            DateTime dateIterator = startDate.Date;


            while (dateIterator.Date != endDate.Date.AddDays(incrementValue))
            {

                if (nonWorkingdays.Contains(dateIterator.Date))
                {
                    endDate = endDate.AddDays(incrementValue);
                }

                dateIterator = dateIterator.AddDays(incrementValue);
            }

            return endDate;
        }

        private DateTime DateIncrement(DateTime startDate, decimal incrementInWorkdays)
        {

            int daysToIncrement = (int)decimal.Truncate(incrementInWorkdays);

            double percentageOfWorkingTime = ((double)((incrementInWorkdays - daysToIncrement) * 100));

            double timeToIncrement = (totalTimeInWorkingDay * percentageOfWorkingTime) / 100;

            return startDate.AddDays(daysToIncrement).AddHours(timeToIncrement);

        }

        private DateTime NormilizeStartDateForIncrement(DateTime startDate, bool incrementIsPositive)
        {

            TimeSpan startTime = new(startHours, startMinutes, 0);
            TimeSpan stopTime = new(stopHours, stopMinutes, 0);

            TimeSpan time = startDate.TimeOfDay;

            if (time > startTime && time < stopTime)
                return startDate;


            if(incrementIsPositive)
            {

                if (time > stopTime)
                    startDate = startDate.AddDays(1);


                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startHours, startMinutes, 0);
            }
            else
            {
                if (time < startTime)
                    startDate = startDate.AddDays(-1);

                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, stopHours, stopMinutes, 0);
            }

            return startDate;
        }

        public void SetHoliday(DateTime date) => holidays.Add(date.Date);

        public void SetRecurringHoliday(int month, int day)
        {
            int lastYear = DateTime.MaxValue.Year;

            for(int year = DateTime.MinValue.Year; year <= lastYear; year++)
            {
                SetHoliday(new DateTime(year: year, month: month, day: day));
            }
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            this.startHours = startHours;
            this.startMinutes = startMinutes;
            this.stopHours = stopHours;
            this.stopMinutes = stopMinutes;

            SetTotalTimeInWorkingDay();
        }

        private void SetWeekends()
        {
            DateTime lastYear = DateTime.MaxValue;

            DateTime date = DateTime.MinValue;

            while(date < lastYear.AddDays(-1))
            {
                if(date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendDays.Add(date);
                }

                date = date.AddDays(1);
            }
        }

        private void SetTotalTimeInWorkingDay()
        {

            double startTime = startHours + (startMinutes * 0.01);
            double stopTime = stopHours + (stopMinutes * 0.01);

            if (startTime > stopTime)
                throw new Exception("Wrong working time configuration");


            totalTimeInWorkingDay = stopTime - startTime;
        }

        
    }
}
