using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Мониторинг_и_прогнозирование_пробок.Models;

namespace Мониторинг_и_прогнозирование_пробок.Data
{
    internal class DBContext: DbContext
    {
        public DbSet<Тип_дня> TypeDays { get; set; }
        public DbSet<Историческая_скорость> HistorySpeeds { get; set; }
        public DbSet<Прогноз_скорости> PrognozSpeeds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-5OBV7BT\SQLEXPRESS;DataBase=Monitoring1;Trusted_Connection=True;TrustServerCertificate=True;");
                
        }
    }
}
