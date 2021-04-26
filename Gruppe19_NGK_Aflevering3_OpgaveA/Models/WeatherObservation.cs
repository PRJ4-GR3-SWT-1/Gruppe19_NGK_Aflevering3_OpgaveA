using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppe19_NGK_Aflevering3_OpgaveA.Models
{
    public class WeatherObservation
    {
        public int WeatherObservationId { get; set; }
        public DateTime Date { get; set; }
        public Location Location { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double AirPressure { get; set; }
    }
}
