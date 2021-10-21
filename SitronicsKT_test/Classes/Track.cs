using System;
using System.Collections.Generic;
using System.Text;

namespace SitronicsKT_test.Classes
{
    class Track
    {
        public Track(int st, int fin, int len)
        {
            Start = st;
            Finish = fin;
            Length = len;
            TrainsList = new List<Train>();
        }
        public int Start { get; set; } //начало пути
        public int Finish { get; set; } //конец пути
        public int Length { get; set; } //длина пути
        public List<Train> TrainsList { get; set; } //поезда, находящиеся здесь в данный момент
    }
}
