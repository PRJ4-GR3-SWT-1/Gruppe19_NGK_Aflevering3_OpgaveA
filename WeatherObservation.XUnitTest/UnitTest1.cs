using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Gruppe19_NGK_Aflevering3_OpgaveA;
using Gruppe19_NGK_Aflevering3_OpgaveA.Controllers;
using Gruppe19_NGK_Aflevering3_OpgaveA.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WeatherObservation = Gruppe19_NGK_Aflevering3_OpgaveA.Models.WeatherObservation;

namespace WeatherObs.XUnitTest
{
    public class UnitTest1
    {

        private readonly WeatherObservationsController uut;
        private readonly MyDbContext dbcontext;

        public UnitTest1()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite(connection).Options;

            dbcontext = new MyDbContext(options);

            uut = new WeatherObservationsController(dbcontext,null);
        }


        [Fact]
        public async void GetWeatherObservation_GetsCorrectObservation()
        {
                dbcontext.Database.EnsureCreated();
                var hugo = new WeatherObservation {Date = DateTime.Now, Location = new Location {Name = "Fredericia"}};
                dbcontext.WeatherObservation.Add(hugo);
                dbcontext.SaveChanges();
            

            var a = await uut.GetWeatherObservationByDate(DateTime.Now);
            Assert.Equal("Fredericia",a.Value.ElementAt(0).Location.Name);
            dbcontext.Database.EnsureDeleted();
        }

        [Fact]
        public async void PostWeatherObservation_CreatesCorrectObservation()
        {
            dbcontext.Database.EnsureCreated();
            var hugo = new WeatherObservation {Date = DateTime.Now, Location = new Location {Name = "København"}};
            await uut.PostWeatherObservation(hugo);

            var a = await uut.GetWeatherObservation();
            List<WeatherObservation> li = a.Value.ToList();
            Assert.Equal("København", li.ElementAt(0).Location.Name);
            dbcontext.Database.EnsureDeleted();
        }

        [Fact]
        public async void GetLast3_GetsTheLast3Observations()
        {
            dbcontext.Database.EnsureCreated();
            var hugo = new WeatherObservation
            {
                Date = new DateTime(2020, 12, 12), Location = new Location {Name = "Hornslet"}
            };
            var hugo2 = new WeatherObservation
            {
                Date = new DateTime(2020, 11, 11), Location = new Location {Name = "København"}
            };
            var hugo3 = new WeatherObservation
            {
                Date = new DateTime(2020, 10, 10), Location = new Location {Name = "Aarhus"}
            };
            var hugo4 = new WeatherObservation
            {
                Date = new DateTime(2000, 12, 12), Location = new Location {Name = "Dårlig By"}
            };
            var hugo5 = new WeatherObservation
            {
                Date = new DateTime(2000, 10, 10), Location = new Location {Name = "Dårlig By 2"}
            };
            dbcontext.WeatherObservation.AddRange(hugo,hugo2,hugo3,hugo4,hugo5);
            dbcontext.SaveChanges();

            var a = await uut.GetLastThreeWeatherObservation();
            List<WeatherObservation> li = a.Value.ToList();

            Assert.Equal(3, li.Count);
            Assert.Collection(
                li,
                item => Assert.Equal("Hornslet",item.Location.Name),
                item => Assert.Equal("København",item.Location.Name),
                item => Assert.Equal("Aarhus",item.Location.Name)
                );
            dbcontext.Database.EnsureDeleted();
        }
        [Fact]
        public async void GetWeatherObservationBetweenDates_ReturnsCorrectObservations()
        {
            dbcontext.Database.EnsureCreated();
            var hugo = new WeatherObservation
            {
                Date = new DateTime(2020, 12, 12), Location = new Location {Name = "Hornslet"}
            };
            var hugo2 = new WeatherObservation
            {
                Date = new DateTime(2020, 11, 11), Location = new Location {Name = "København"}
            };
            var hugo3 = new WeatherObservation
            {
                Date = new DateTime(2020, 10, 10), Location = new Location {Name = "Aarhus"}
            };
            var hugo4 = new WeatherObservation
            {
                Date = new DateTime(2000, 12, 12), Location = new Location {Name = "Dårlig By"}
            };
            var hugo5 = new WeatherObservation
            {
                Date = new DateTime(2000, 10, 10), Location = new Location {Name = "Dårlig By 2"}
            };
            dbcontext.WeatherObservation.AddRange(hugo, hugo2, hugo3, hugo4, hugo5);
            dbcontext.SaveChanges();

            var a = await uut.GetWeatherObservationBetweenDate(new DateTime(2000,10,10),new DateTime(2020,10,10));
            List<WeatherObservation> li = a.Value.ToList();

            Assert.Equal(3, li.Count);
            Assert.Collection(
                li,
                item => Assert.Equal("Aarhus", item.Location.Name),
                item => Assert.Equal("Dårlig By",item.Location.Name),
                item => Assert.Equal("Dårlig By 2", item.Location.Name)
            );

            dbcontext.Database.EnsureDeleted();
        }

        [Fact]
        public async void GetWeatherObservationByDate_ReturnsCorrectObservation()
        {
            dbcontext.Database.EnsureCreated();
            var hugo = new WeatherObservation
            {
                Date = new DateTime(2020, 12, 12), Location = new Location {Name = "Hornslet"}
            };
            var hugo2 = new WeatherObservation
            {
                Date = new DateTime(2020, 12, 12), Location = new Location {Name = "København"}
            };
            var hugo3 = new WeatherObservation
            {
                Date = new DateTime(2020, 10, 10), Location = new Location {Name = "Aarhus"}
            };
            dbcontext.WeatherObservation.AddRange(hugo, hugo2, hugo3);
            dbcontext.SaveChanges();

            var a = await uut.GetWeatherObservationByDate( new DateTime(2020, 12, 12));
            List<WeatherObservation> li = a.Value.ToList();

            Assert.Equal(2,li.Count);
            Assert.Collection(
                li,
                item => Assert.Equal("Hornslet", item.Location.Name),
                item => Assert.Equal("København", item.Location.Name)
            );

            dbcontext.Database.EnsureDeleted();
        }
    }
}
