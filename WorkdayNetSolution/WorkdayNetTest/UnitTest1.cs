using System;
using WorkdayNet.Service;
using Xunit;

namespace WorkdayNetTest
{
    public class UnitTest1
    {

        [Fact]
        public void TestCase1Ok()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));

            var start = new DateTime(2004, 5, 24, 18, 5, 0);

            decimal increment = -5.5m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 5, 14, 12, 0, 0);


            Assert.Equal(realResult, expectedResult);

        }


        [Fact]
        public void TestCase1NoOk()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));

            var start = new DateTime(2004, 5, 24, 18, 5, 0);

            decimal increment = -5.5m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 5, 17, 12, 0, 0);


            Assert.NotEqual(realResult, expectedResult);

        }


        [Fact]
        public void TestCase2Ok()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));


            var start = new DateTime(2004, 5, 24, 19, 3, 0);

            decimal increment = 44.723656m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 7, 27, 13, 47, 0);


            Assert.Equal(realResult, expectedResult);

        }

        [Fact]
        public void TestCase3Ok()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));


            var start = new DateTime(2004, 5, 24, 18, 3, 0);

            decimal increment = -6.7470217m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 5, 13, 10, 2, 0);


            Assert.Equal(realResult, expectedResult);

        }

        [Fact]
        public void TestCase4Ok()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));


            var start = new DateTime(2004, 5, 24, 8, 3, 0);

            decimal increment = 12.782709m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 6, 10, 14, 18, 0);


            Assert.Equal(realResult, expectedResult);

        }


        [Fact]
        public void TestCase5Ok()
        {
            IWorkdayCalendar sut = new WorkdayCalendar();

            sut.SetWorkdayStartAndStop(8, 0, 16, 0);

            sut.SetRecurringHoliday(5, 17);

            sut.SetHoliday(new DateTime(2004, 5, 27));


            var start = new DateTime(2004, 5, 24, 7, 3, 0);

            decimal increment = 8.276628m;


            DateTime realResult = sut.GetWorkdayIncrement(start, increment);
            DateTime expectedResult = new DateTime(2004, 6, 4, 10, 12, 0);


            Assert.Equal(realResult, expectedResult);

        }

    }
}


