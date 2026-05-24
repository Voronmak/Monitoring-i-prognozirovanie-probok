using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Мониторинг_и_прогнозирование_пробок.Models
{
    internal class Тип_дня
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool prazdnik { get; set; }
        public int Nagruzki { get; set; }
        public int IDPrognoz { get; set; }

        public Прогноз_скорости? Прогноз_Скорости { get; set; }
    }
}
