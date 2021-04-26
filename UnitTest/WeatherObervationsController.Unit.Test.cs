using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UnitTest
{
    public class WeatherObervationsControllerUnitTest
    {
        private WeatherObervationsController uut;
        private DbContext dbc;
        [SetUp]
        public void Setup()
        {
            uut = new WeatherObervationsController();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}