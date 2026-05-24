using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Мониторинг_и_прогнозирование_пробок.Models
{
    internal class Прогноз_скорости
    {
        public int ID { get; set; }
        public DateTime PrognozaDataVremya { get; set; }
        public DateTime DataSozdaniaPrognoza { get; set; }
        public int PrognozSpeed { get; set; }

        
    }
}
