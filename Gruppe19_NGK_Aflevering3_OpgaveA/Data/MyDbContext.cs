using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gruppe19_NGK_Aflevering3_OpgaveA.Models;

    public class MyDbContext : DbContext
    {
        public MyDbContext (DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Gruppe19_NGK_Aflevering3_OpgaveA.Models.WeatherObservation> WeatherObservation { get; set; }
    }
