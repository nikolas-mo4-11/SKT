using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SitronicsKT_test.Classes;

namespace SitronicsKT_test
{
    class Program
    {
        // функция для обновления состояния. Возвращает:
        // 1, если все поезда закончили маршруты
        // 0, если ещё ездят
        // -1, если произошло столкновение
        public static int UpdateState(List<Train> trains, List<Track> tracks, List<Station> stations)
        {
            // 1. проверка начального состояния на станциях: если где-то стоит 2 поезда - выводим сообщение о столкновении
            if (stations.Where(station => station.TrainsList.Count() > 1).Count() > 0) return -1;


            // 2. если поезд стоит на станции - помещаем его на нужный трек + удаляем все поезда со станции
            foreach (var station_el in stations)
            {
                foreach (var train_el in station_el.TrainsList)
                {
                    if (train_el.CurrentTrackFinish != -1) //если он не закончил свой маршрут - размещаем его на нужном пути
                        foreach (var track_el in tracks)
                            if (track_el.Start == train_el.CurrentTrackStart && track_el.Finish == train_el.CurrentTrackFinish
                                || track_el.Start == train_el.CurrentTrackFinish && track_el.Finish == train_el.CurrentTrackStart)
                            {
                                Train temp = new Train(train_el.StationList, track_el.Length);
                                track_el.TrainsList.Add(temp);
                                trains.Add(temp);
                                break;
                            }
                    trains.Remove(train_el);
                }
                station_el.TrainsList.Clear();
            }

            // 3. предположим, что мы поедем. Что-то случится? Проверяем каждую пару поездов на каждом треке
            foreach (var track_el in tracks)
                for (int i = 0; i < track_el.TrainsList.Count(); i++)
                    for (int j = i+1; j < track_el.TrainsList.Count(); j++)
                    {
                        //проверка не окажуся ли они в одной точке
                        if (track_el.TrainsList[i].DirectionOnCurrentTrack != track_el.TrainsList[j].DirectionOnCurrentTrack &&
                          track_el.TrainsList[i].CurrentTrackLocate + 1 == track_el.Length - (track_el.TrainsList[j].CurrentTrackLocate + 1))
                            return -1;

                        //проверка не поменяются ли они местами
                        if (track_el.TrainsList[i].DirectionOnCurrentTrack != track_el.TrainsList[j].DirectionOnCurrentTrack &&
                          track_el.TrainsList[i].CurrentTrackLocate + 1 == track_el.Length - track_el.TrainsList[j].CurrentTrackLocate &&
                          track_el.TrainsList[i].CurrentTrackLocate == track_el.Length - (track_el.TrainsList[j].CurrentTrackLocate + 1))
                            return -1;
                    }


            // 4. продвинули каждый поезд дальше
            trains.ForEach(train_el => train_el.CurrentTrackLocate++);

            // 5. если какой-то поезд дошёл до конца трека - добавить на станцию и удалить с текущего пути
            foreach (var track_el in tracks)
                while (true)
                    try
                    {
                        //нашли очередной поезд, который нужно удалить на данном треке
                        Train fin_train = track_el.TrainsList.First(t => t.CurrentTrackLength == t.CurrentTrackLocate); 
                        fin_train.StationList.RemoveAt(0); //удаляем первую станцию в маршруте этого поезда

                        Train temp = new Train(fin_train.StationList);

                        stations[fin_train.CurrentTrackStart].TrainsList.Add(temp);
                        trains.Add(temp);

                        track_el.TrainsList.Remove(fin_train);
                        trains.Remove(fin_train);
                    }
                    catch (InvalidOperationException e)
                    {
                        //как только таких поездов не осталось
                        break;
                    }

            // 6. если у нас не осталось движущихся поездов - всё закончилось без происшествий
            return (trains.Count() == 0) ? 1 : 0;
        }

        //функция для чтения маршрутов поездов
        static List<Train> TrainsRead()
        {
            string dir1 = "C:/Users/Nikolas/source/repos/SitronicsKT_test/input_data/trains_paths.txt";
            string[] lines = File.ReadAllLines(dir1);

            return lines
                .Select(line => new Train(line.Split(' ').Select(l => int.Parse(l)).ToList()))
                .ToList();
        }

        //функция для чтения матрицы путей
        static void TracksRead(List<Track> tracks, List<Station> stations)
        {
            string dir2 = "C:/Users/Nikolas/source/repos/SitronicsKT_test/input_data/tracks_matrix.txt";
            string[] lines = File.ReadAllLines(dir2);

            int stations_counts = int.Parse(lines[0]);

            for (int i = 0; i < stations_counts; i++)
            {
                stations.Add(new Station());
                for (int j = 0; j < stations_counts; j++)
                    if (i < j) tracks.Add(new Track(i, j, int.Parse(lines[i + 1].Split(' ')[j])));
            }
        }

        static void Main(string[] args)
        {
            //для поддержания состояния храним 3 массива: поезда, пути, станции
            List<Train> trains;
            List<Track> tracks = new List<Track>();
            List<Station> stations = new List<Station>();

            //читаем маршруты поездов из файла
            trains = TrainsRead();

            //загружаем матрицу путей
            TracksRead(tracks, stations);

            //размещаем поезда на начальных станциях
            foreach (var train_el in trains)
                stations[train_el.CurrentTrackStart].TrainsList.Add(train_el);

            //меняем состояния
            int flag = 0;
            while (flag == 0)
                flag = UpdateState(trains, tracks, stations);
            if (flag == 1) Console.WriteLine("Всё прекрасно!");
            else Console.WriteLine("Произошло столкновение"); ;
        }
    }
}