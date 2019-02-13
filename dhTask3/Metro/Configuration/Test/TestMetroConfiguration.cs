using Metro.Configuration;
using System;
using System.Collections.Generic;

namespace Metro.Configuration.Test
{
    static class TestMetroConfiguration
    {
        public static XmlMetroConfiguration GetMoscowMetroTempConfiguration()
        {
            XmlMetroConfiguration configuration = new XmlMetroConfiguration();
            configuration.MetroName = "Московский метрополитен";
            configuration.Lines = new List<LineConfiguration>();
            configuration.Lines.Add(new LineConfiguration() { Id = 1, Name = "Сокольническая", ColorId = (int)ConsoleColor.Red });
            configuration.Lines.Add(new LineConfiguration() { Id = 2, Name = "Замоскворецкая", ColorId = (int)ConsoleColor.Green });
            configuration.Lines.Add(new LineConfiguration() { Id = 3, Name = "Арбатско-Покровская", ColorId = (int)ConsoleColor.Cyan });
            configuration.Lines.Add(new LineConfiguration() { Id = 4, Name = "Филёвская", ColorId = (int)ConsoleColor.Blue });
            configuration.Lines.Add(new LineConfiguration() { Id = 5, Name = "Кольцевая", ColorId = (int)ConsoleColor.Black });

            configuration.Stations = new List<StationConfiguration>();

            int lineId = 1;
            int id = 1001;
            StationConfiguration frunzenskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            frunzenskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Фрунзенская" });
            configuration.Stations.Add(frunzenskaya);


            id++;
            StationConfiguration parkKultury = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            parkKultury.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Парк Культуры" });
            parkKultury.NeighboursStationIds.Add(frunzenskaya.Id);
            frunzenskaya.NeighboursStationIds.Add(parkKultury.Id);
            configuration.Stations.Add(parkKultury);

            id++;
            StationConfiguration kropotkinskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            kropotkinskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Кропоткинская" });
            kropotkinskaya.NeighboursStationIds.Add(parkKultury.Id);
            parkKultury.NeighboursStationIds.Add(kropotkinskaya.Id);
            configuration.Stations.Add(kropotkinskaya);

            id++;
            StationConfiguration abab = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            abab.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Библиотека им. Ленина" });
            abab.LineStations.Add(new LineStationConfiguration() { LineId = 3, StationName = "Арбатская" });
            abab.LineStations.Add(new LineStationConfiguration() { LineId = 4, StationName = "Александровский сад" });
            abab.NeighboursStationIds.Add(kropotkinskaya.Id);
            kropotkinskaya.NeighboursStationIds.Add(abab.Id);
            configuration.Stations.Add(abab);

            id++;
            StationConfiguration otpr = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            otpr.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Охотный ряд" });
            otpr.LineStations.Add(new LineStationConfiguration() { LineId = 2, StationName = "Театральная" });
            otpr.LineStations.Add(new LineStationConfiguration() { LineId = 3, StationName = "Площадь революции" });
            otpr.NeighboursStationIds.Add(abab.Id);
            abab.NeighboursStationIds.Add(otpr.Id);
            configuration.Stations.Add(otpr);

            id++;
            StationConfiguration lubyanka = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            lubyanka.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Лубянка" });
            lubyanka.NeighboursStationIds.Add(otpr.Id);
            otpr.NeighboursStationIds.Add(lubyanka.Id);
            configuration.Stations.Add(lubyanka);

            id++;
            StationConfiguration tcc = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            tcc.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Чистые пруды" });
            tcc.NeighboursStationIds.Add(lubyanka.Id);
            lubyanka.NeighboursStationIds.Add(tcc.Id);
            configuration.Stations.Add(tcc);

            id++;
            StationConfiguration krasnye = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            krasnye.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Красные ворота" });
            krasnye.NeighboursStationIds.Add(tcc.Id);
            tcc.NeighboursStationIds.Add(krasnye.Id);
            configuration.Stations.Add(krasnye);

            id++;
            StationConfiguration komsomolskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            komsomolskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Комсомольская" });
            komsomolskaya.LineStations.Add(new LineStationConfiguration() { LineId = 5, StationName = "Комсомольская" });
            komsomolskaya.NeighboursStationIds.Add(krasnye.Id);
            krasnye.NeighboursStationIds.Add(komsomolskaya.Id);
            configuration.Stations.Add(komsomolskaya);

            id++;
            StationConfiguration krasnoselskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            krasnoselskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Красносельская" });
            krasnoselskaya.NeighboursStationIds.Add(komsomolskaya.Id);
            komsomolskaya.NeighboursStationIds.Add(krasnoselskaya.Id);
            configuration.Stations.Add(krasnoselskaya);

            id++;
            StationConfiguration preobrazhenkaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            preobrazhenkaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Преображенская площадь" });
            preobrazhenkaya.NeighboursStationIds.Add(krasnoselskaya.Id);
            krasnoselskaya.NeighboursStationIds.Add(preobrazhenkaya.Id);
            configuration.Stations.Add(preobrazhenkaya);

            lineId = 2;
            id = 2001;
            StationConfiguration paveleckaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            paveleckaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Павелецкая" });
            configuration.Stations.Add(paveleckaya);

            id++;
            StationConfiguration tretyakovskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            tretyakovskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Новокузнецкая" });
            tretyakovskaya.NeighboursStationIds.Add(paveleckaya.Id);
            paveleckaya.NeighboursStationIds.Add(tretyakovskaya.Id);
            configuration.Stations.Add(tretyakovskaya);

            otpr.NeighboursStationIds.Add(tretyakovskaya.Id);
            tretyakovskaya.NeighboursStationIds.Add(otpr.Id);

            id++;
            StationConfiguration tpc = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            tpc.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Тверская" });
            tpc.NeighboursStationIds.Add(otpr.Id);
            otpr.NeighboursStationIds.Add(tpc.Id);
            configuration.Stations.Add(tpc);

            id++;
            StationConfiguration mayakovskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            mayakovskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Маяковская" });
            mayakovskaya.NeighboursStationIds.Add(tpc.Id);
            tpc.NeighboursStationIds.Add(mayakovskaya.Id);
            configuration.Stations.Add(mayakovskaya);

            id++;
            StationConfiguration belorusskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            belorusskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Белорусская" });
            belorusskaya.LineStations.Add(new LineStationConfiguration() { LineId = 5, StationName = "Белорусская" });
            belorusskaya.NeighboursStationIds.Add(mayakovskaya.Id);
            mayakovskaya.NeighboursStationIds.Add(belorusskaya.Id);
            configuration.Stations.Add(belorusskaya);

            id++;
            StationConfiguration dinamo = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            dinamo.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Динамо" });
            dinamo.NeighboursStationIds.Add(belorusskaya.Id);
            belorusskaya.NeighboursStationIds.Add(dinamo.Id);
            configuration.Stations.Add(dinamo);

            id++;
            StationConfiguration aeroport = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            aeroport.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Аэропорт" });
            aeroport.NeighboursStationIds.Add(dinamo.Id);
            dinamo.NeighboursStationIds.Add(aeroport.Id);
            configuration.Stations.Add(aeroport);

            id++;
            StationConfiguration sokol = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            sokol.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Сокол" });
            sokol.NeighboursStationIds.Add(aeroport.Id);
            aeroport.NeighboursStationIds.Add(sokol.Id);
            configuration.Stations.Add(sokol);

            lineId = 3;
            id = 3001;
            StationConfiguration kunceevskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            kunceevskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Кунцевская" });
            kunceevskaya.LineStations.Add(new LineStationConfiguration() { LineId = 4, StationName = "Кунцевская" });
            configuration.Stations.Add(kunceevskaya);

            id++;
            StationConfiguration slavyanskyBulvar = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            slavyanskyBulvar.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Славянский бульвар" });
            slavyanskyBulvar.NeighboursStationIds.Add(kunceevskaya.Id);
            kunceevskaya.NeighboursStationIds.Add(slavyanskyBulvar.Id);
            configuration.Stations.Add(slavyanskyBulvar);

            id++;
            StationConfiguration parkPobedy = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            parkPobedy.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Парк победы" });
            parkPobedy.NeighboursStationIds.Add(slavyanskyBulvar.Id);
            slavyanskyBulvar.NeighboursStationIds.Add(parkPobedy.Id);
            configuration.Stations.Add(parkPobedy);

            id++;
            StationConfiguration kyivskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            kyivskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Киевская" });
            kyivskaya.LineStations.Add(new LineStationConfiguration() { LineId = 4, StationName = "Киевская" });
            kyivskaya.LineStations.Add(new LineStationConfiguration() { LineId = 5, StationName = "Киевская" });
            kyivskaya.NeighboursStationIds.Add(parkPobedy.Id);
            parkPobedy.NeighboursStationIds.Add(kyivskaya.Id);
            configuration.Stations.Add(kyivskaya);

            id++;
            StationConfiguration smolenskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            smolenskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Смоленская" });
            smolenskaya.NeighboursStationIds.Add(parkPobedy.Id);
            parkPobedy.NeighboursStationIds.Add(smolenskaya.Id);
            configuration.Stations.Add(smolenskaya);

            abab.NeighboursStationIds.Add(smolenskaya.Id);
            smolenskaya.NeighboursStationIds.Add(abab.Id);

            id++;
            StationConfiguration kurskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            kurskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Курская" });
            kurskaya.LineStations.Add(new LineStationConfiguration() { LineId = 5, StationName = "Курская" });
            kurskaya.NeighboursStationIds.Add(otpr.Id);
            otpr.NeighboursStationIds.Add(kurskaya.Id);
            configuration.Stations.Add(kurskaya);

            id++;
            StationConfiguration baumanska = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            baumanska.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Бауманская" });
            baumanska.NeighboursStationIds.Add(kurskaya.Id);
            kurskaya.NeighboursStationIds.Add(baumanska.Id);
            configuration.Stations.Add(baumanska);

            id++;
            StationConfiguration electrozavodskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            electrozavodskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Электрозаводская" });
            electrozavodskaya.NeighboursStationIds.Add(baumanska.Id);
            baumanska.NeighboursStationIds.Add(electrozavodskaya.Id);
            configuration.Stations.Add(electrozavodskaya);

            id++;
            StationConfiguration semenovskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            semenovskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Семёновская" });
            semenovskaya.NeighboursStationIds.Add(electrozavodskaya.Id);
            electrozavodskaya.NeighboursStationIds.Add(semenovskaya.Id);
            configuration.Stations.Add(semenovskaya);


            id = 4001;
            lineId = 4;
            StationConfiguration pionerskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            pionerskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Прионерская" });
            pionerskaya.NeighboursStationIds.Add(kunceevskaya.Id);
            kunceevskaya.NeighboursStationIds.Add(pionerskaya.Id);
            configuration.Stations.Add(pionerskaya);

            id++;
            StationConfiguration filevskyPark = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            filevskyPark.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Филёвский парк" });
            filevskyPark.NeighboursStationIds.Add(pionerskaya.Id);
            pionerskaya.NeighboursStationIds.Add(filevskyPark.Id);
            configuration.Stations.Add(filevskyPark);

            id++;
            StationConfiguration bagrationovskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            bagrationovskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Багратионовская" });
            bagrationovskaya.NeighboursStationIds.Add(filevskyPark.Id);
            filevskyPark.NeighboursStationIds.Add(bagrationovskaya.Id);
            configuration.Stations.Add(bagrationovskaya);

            id++;
            StationConfiguration fili = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            fili.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Фили" });
            fili.NeighboursStationIds.Add(bagrationovskaya.Id);
            bagrationovskaya.NeighboursStationIds.Add(fili.Id);
            configuration.Stations.Add(fili);

            id++;
            StationConfiguration kutuzovskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            kutuzovskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Кутузовская" });
            kutuzovskaya.NeighboursStationIds.Add(fili.Id);
            fili.NeighboursStationIds.Add(kutuzovskaya.Id);
            configuration.Stations.Add(kutuzovskaya);

            id++;
            StationConfiguration studencheskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            studencheskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Студенческая" });
            studencheskaya.NeighboursStationIds.Add(kutuzovskaya.Id);
            kutuzovskaya.NeighboursStationIds.Add(studencheskaya.Id);
            configuration.Stations.Add(studencheskaya);

            kyivskaya.NeighboursStationIds.Add(studencheskaya.Id);
            studencheskaya.NeighboursStationIds.Add(kyivskaya.Id);

            id++;
            StationConfiguration smolenskaya2 = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            smolenskaya2.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Смоленская" });
            smolenskaya2.NeighboursStationIds.Add(kyivskaya.Id);
            kyivskaya.NeighboursStationIds.Add(smolenskaya2.Id);
            configuration.Stations.Add(smolenskaya2);

            id++;
            StationConfiguration arbatskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            arbatskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Арбатская" });
            arbatskaya.NeighboursStationIds.Add(smolenskaya2.Id);
            smolenskaya2.NeighboursStationIds.Add(arbatskaya.Id);
            configuration.Stations.Add(arbatskaya);

            abab.NeighboursStationIds.Add(arbatskaya.Id);
            arbatskaya.NeighboursStationIds.Add(abab.Id);

            lineId = 5;
            id = 5001;
            StationConfiguration krasnopresnenskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            krasnopresnenskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Краснопресненская" });
            krasnopresnenskaya.NeighboursStationIds.Add(kyivskaya.Id);
            kyivskaya.NeighboursStationIds.Add(krasnopresnenskaya.Id);
            configuration.Stations.Add(krasnopresnenskaya);

            belorusskaya.NeighboursStationIds.Add(krasnopresnenskaya.Id);
            krasnopresnenskaya.NeighboursStationIds.Add(belorusskaya.Id);

            id++;
            StationConfiguration mendeleevskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            mendeleevskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Менделеевская" });
            mendeleevskaya.NeighboursStationIds.Add(belorusskaya.Id);
            belorusskaya.NeighboursStationIds.Add(mendeleevskaya.Id);
            configuration.Stations.Add(mendeleevskaya);

            id++;
            StationConfiguration prospektMira = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            prospektMira.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Проспект Мира" });
            prospektMira.NeighboursStationIds.Add(mendeleevskaya.Id);
            mendeleevskaya.NeighboursStationIds.Add(prospektMira.Id);
            configuration.Stations.Add(prospektMira);

            komsomolskaya.NeighboursStationIds.Add(prospektMira.Id);
            prospektMira.NeighboursStationIds.Add(komsomolskaya.Id);

            kurskaya.NeighboursStationIds.Add(komsomolskaya.Id);
            komsomolskaya.NeighboursStationIds.Add(kurskaya.Id);

            id++;
            StationConfiguration taganskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            taganskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Таганская" });
            taganskaya.NeighboursStationIds.Add(kurskaya.Id);
            kurskaya.NeighboursStationIds.Add(taganskaya.Id);
            configuration.Stations.Add(taganskaya);

            paveleckaya.NeighboursStationIds.Add(taganskaya.Id);
            taganskaya.NeighboursStationIds.Add(paveleckaya.Id);

            id++;
            StationConfiguration dobryninskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            dobryninskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Добрынинская" });
            dobryninskaya.NeighboursStationIds.Add(paveleckaya.Id);
            paveleckaya.NeighboursStationIds.Add(dobryninskaya.Id);
            configuration.Stations.Add(dobryninskaya);

            id++;
            StationConfiguration oktyabrskaya = new StationConfiguration() { Id = id, LineStations = new List<LineStationConfiguration>(), NeighboursStationIds = new List<int>() };
            oktyabrskaya.LineStations.Add(new LineStationConfiguration() { LineId = lineId, StationName = "Октябрьская" });
            oktyabrskaya.NeighboursStationIds.Add(dobryninskaya.Id);
            dobryninskaya.NeighboursStationIds.Add(oktyabrskaya.Id);
            configuration.Stations.Add(oktyabrskaya);

            parkKultury.NeighboursStationIds.Add(dobryninskaya.Id);
            dobryninskaya.NeighboursStationIds.Add(parkKultury.Id);

            parkKultury.NeighboursStationIds.Add(kyivskaya.Id);
            kyivskaya.NeighboursStationIds.Add(parkKultury.Id);

            return configuration;
        }
    }
}