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

        private WeatherObservationsController uut;
        private MyDbContext dbcontext;

        public UnitTest1()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite(connection).Options;

            dbcontext = new MyDbContext(options);

            uut = new WeatherObservationsController(dbcontext);
        }


        [Fact]
        public async void GetWeatherObservation_GetsCorrectObservation()
        {
                dbcontext.Database.EnsureCreated();
                var hugo = new WeatherObservation();
                hugo.Date = DateTime.Now;
                hugo.Location = new Location();
                hugo.Location.Name = "Fredericia";
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
            var hugo = new WeatherObservation();
            hugo.Date = DateTime.Now;
            hugo.Location = new Location();
            hugo.Location.Name = "København";
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
            var hugo = new WeatherObservation();
            hugo.Date = new DateTime(2020, 12, 12);
            hugo.Location = new Location();
            hugo.Location.Name = "Hornslet";
            var hugo2 = new WeatherObservation();
            hugo2.Date = new DateTime(2020, 11, 11);
            hugo2.Location = new Location();
            hugo2.Location.Name = "København";
            var hugo3 = new WeatherObservation();
            hugo3.Date = new DateTime(2020, 10, 10);
            hugo3.Location = new Location();
            hugo3.Location.Name = "Aarhus";
            var hugo4 = new WeatherObservation();
            hugo4.Date = new DateTime(2000,12,12);
            hugo4.Location = new Location();
            hugo4.Location.Name = "Dårlig By";
            var hugo5 = new WeatherObservation();
            hugo5.Date = new DateTime(2000, 10, 10);
            hugo5.Location = new Location();
            hugo5.Location.Name = "Dårlig By 2";
            dbcontext.WeatherObservation.AddRange(hugo,hugo2,hugo3,hugo4,hugo5);
            dbcontext.SaveChanges();

            var a = await uut.GetLastThreeWeatherObservation();
            List<WeatherObservation> li = a.Value.ToList();

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
            var hugo = new WeatherObservation();
            hugo.Date = new DateTime(2020, 12, 12);
            hugo.Location = new Location();
            hugo.Location.Name = "Hornslet";
            var hugo2 = new WeatherObservation();
            hugo2.Date = new DateTime(2020, 11, 11);
            hugo2.Location = new Location();
            hugo2.Location.Name = "København";
            var hugo3 = new WeatherObservation();
            hugo3.Date = new DateTime(2020, 10, 10);
            hugo3.Location = new Location();
            hugo3.Location.Name = "Aarhus";
            var hugo4 = new WeatherObservation();
            hugo4.Date = new DateTime(2000, 12, 12);
            hugo4.Location = new Location();
            hugo4.Location.Name = "Dårlig By";
            var hugo5 = new WeatherObservation();
            hugo5.Date = new DateTime(2000, 10, 10);
            hugo5.Location = new Location();
            hugo5.Location.Name = "Dårlig By 2";
            dbcontext.WeatherObservation.AddRange(hugo, hugo2, hugo3, hugo4, hugo5);
            dbcontext.SaveChanges();

            var a = await uut.GetWeatherObservationBetweenDate(new DateTime(2000,10,10),new DateTime(2020,10,10));
            List<WeatherObservation> li = a.Value.ToList();

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
            var hugo = new WeatherObservation();
            hugo.Date = new DateTime(2020, 12, 12);
            hugo.Location = new Location();
            hugo.Location.Name = "Hornslet";
            var hugo2 = new WeatherObservation();
            hugo2.Date = new DateTime(2020, 12, 12);
            hugo2.Location = new Location();
            hugo2.Location.Name = "København";
            var hugo3 = new WeatherObservation();
            hugo3.Date = new DateTime(2020, 10, 10);
            hugo3.Location = new Location();
            hugo3.Location.Name = "Aarhus";
            dbcontext.WeatherObservation.AddRange(hugo, hugo2, hugo3);
            dbcontext.SaveChanges();

            var a = await uut.GetWeatherObservationByDate( new DateTime(2020, 12, 12));
            List<WeatherObservation> li = a.Value.ToList();

            Assert.Collection(
                li,
                item => Assert.Equal("Hornslet", item.Location.Name),
                item => Assert.Equal("København", item.Location.Name)
            );

            dbcontext.Database.EnsureDeleted();
        }
    }
}
