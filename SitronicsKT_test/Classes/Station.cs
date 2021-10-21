using System;
using System.Collections.Generic;
using System.Text;

namespace SitronicsKT_test.Classes
{
    class Station
    {
        public Station()
        {
            TrainsList = new List<Train>();
        }
        public List<Train> TrainsList { get; set; }
    }
}
