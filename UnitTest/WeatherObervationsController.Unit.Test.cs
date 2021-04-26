using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Gruppe19_NGK_Aflevering3_OpgaveA.Controllers;
using NSubstitute;

namespace UnitTest
{
    public class WeatherObervationsControllerUnitTest
    {
        private WeatherObservationsController uut;
        private MyDbContext dbc;
        [SetUp]
        public void Setup()
        {
            dbc = Substitute.For<MyDbContext>();
            uut = new WeatherObservationsController(dbc);
        }

        [Test]
        public void Test1()
        {
            uut.GetWeatherObservationByDate(DateTime.Now).Wait();
            dbc.Received(1).WeatherObservation.Where(i => i.Date.Date == DateTime.Now).ToListAsync();
        }
    }
}