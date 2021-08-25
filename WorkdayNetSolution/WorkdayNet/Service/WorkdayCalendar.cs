using System;
using System.Collections.Generic;

namespace WorkdayNet.Service
{
    public class WorkdayCalendar : IWorkdayCalendar
    {

        private List<DateTime> holidays;
        private List<DateTime> weekendDays;

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

            startDate = NormilizeStartDateForIncrement(startDate);

            DateTime endDate = DateIncrement(startDate, incrementInWorkdays);

            endDate = adaptIncrementToHolidaysAndWeekendDays(startDate, endDate);

            return endDate;
            
        }


        private DateTime adaptIncrementToHolidaysAndWeekendDays(DateTime startDate, DateTime endDate)
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

            int percentageOfWorkingTime = (int)((incrementInWorkdays - daysToIncrement) * 100);

            double timeToIncrement = (totalTimeInWorkingDay * percentageOfWorkingTime) / 100;

            return startDate.AddDays(daysToIncrement).AddHours(timeToIncrement);

        }

        private DateTime NormilizeStartDateForIncrement(DateTime startDate)
        {

            TimeSpan startTime = new(startHours, startMinutes, 0);
            TimeSpan stopTime = new(stopHours, stopMinutes, 0);

            TimeSpan time = startDate.TimeOfDay;

            if (time < startTime)
            {
                startDate = startDate.Add(time - startTime);
            }

            if (time > stopTime)
            {
                startDate = startDate.Add(stopTime - time);
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
