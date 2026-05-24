using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Мониторинг_и_прогнозирование_пробок.Models
{
    internal class Историческая_скорость
    {
        public int ID { get; set; }
        public DateTime DataVremya { get; set; }
        public decimal SrScorost { get; set; }
        public decimal ScorostMedlPotoka { get; set; }
        public decimal ScorostBystrPotoka { get; set; }
        public int IDPrognoza { get; set; }
        public Прогноз_скорости? Прогноз_скорости { get; set; }
    }
}
