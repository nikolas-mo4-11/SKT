using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SitronicsKT_test
{
    class Train
    {
        public Train(List<int> stations = default, int new_track_len = 0)
        {
            StationList = stations;
            CurrentTrackStart = StationList[0];
            CurrentTrackFinish = (StationList.Count() > 1) ? StationList[1] : -1;
            CurrentTrackLocate = 0;
            CurrentTrackLength = new_track_len;
            DirectionOnCurrentTrack = (CurrentTrackStart < CurrentTrackFinish) ? 1 : -1;
        }

        public List<int> StationList { get; set; } //маршрут поезда
        public int CurrentTrackStart { get; set; } //начальный пункт текущего участка пути
        public int CurrentTrackFinish { get; set; } //конечный пункт текущего перегона
        public int CurrentTrackLocate { get; set; } //текущее положение на участке пути [0, len]
        public int CurrentTrackLength { get; set; } //длина текущего участка пути
        public int DirectionOnCurrentTrack { get; set; } // направление движения (исходя из конфигурации жд): -1 или 1
    }
}
