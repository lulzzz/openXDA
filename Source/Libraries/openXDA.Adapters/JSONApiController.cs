﻿//******************************************************************************************************
//  JSONApiController.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/08/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using GSF.Collections;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA.Adapters
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JSONApiController : ApiController
    {

        #region [Query Classes]
        public class ConfigJSON
        {
            public string AssetKey { get; set; }
            public string ID { get; set; } 
            public string Name { get; set; }
        }

        public class EventJSON
        {
            public string MeterIDList { get; set; }
            public string MeterAssetKeyList { get; set; }
            public string EventIDList { get; set; }
            public string LineIDList { get; set; }
            public string LineAssetKeyList { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

        }

        public class EventDataJSON
        {
            public string EventID { get; set; }
        }

        #endregion

        #region [Config Calls]
        [HttpPost]
        public IEnumerable<Meter> GetMeters(ConfigJSON json)
        {
            using(DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                int id = (json != null ? (json.ID == null ? -1: int.Parse(json.ID)) : -1);
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Meter WHERE AssetKey LIKE {0} AND "+ (id != -1? "ID LIKE {1} AND ": "" )+ "NAME LIKE {2}", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<Meter>().LoadRecord(row)).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Line> GetLines(ConfigJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                int id = (json != null ? (json.ID == null ? -1 : int.Parse(json.ID)) : -1);

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Line WHERE AssetKey LIKE {0} " + (id != -1 ? "AND ID LIKE {1}" : "") , assetKey, id);
                return table.Select().Select(row => dataContext.Table<Line>().LoadRecord(row)).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<MeterLocation> GetStations(ConfigJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                int id = (json != null ? (json.ID == null ? -1 : int.Parse(json.ID)) : -1);
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM MeterLocation WHERE AssetKey LIKE {0} AND " + (id != -1 ? "ID LIKE {1} AND " : "") + "NAME LIKE {2} ", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<MeterLocation>().LoadRecord(row)).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Channel> GetChannelsByMeter(ConfigJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                int id = (json != null ? (json.ID == null ? -1 : int.Parse(json.ID)) : -1);
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Channel WHERE MeterID IN (Select ID FROM Meter WHERE AssetKey LIKE {0} AND " + (id != -1 ? "ID LIKE {1} AND " : "") + "NAME LIKE {2})", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<Channel>().LoadRecord(row)).ToList();
            }
        }


        #endregion

        #region [Event Calls]
        [HttpPost]
        public IEnumerable<Event> GetEvents(EventJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                DateTime startTime = (json != null ? (json.StartDate ?? DateTime.Parse("01/01/01")) : DateTime.Parse("01/01/01"));
                DateTime endTime = (json != null ? (json.EndDate ?? DateTime.Now) : DateTime.Now);

                IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();
                try
                {
                    if (json == null || (json.EventIDList == null && json.MeterAssetKeyList == null && json.LineIDList == null && json.LineAssetKeyList == null && json.MeterIDList == null))
                    {
                        table = dataContext.Connection.RetrieveData("Select * FROM Event WHERE StartTime >= {0} AND EndTime <= {1}", startTime, endTime).Select();
                        return table.Select(row => dataContext.Table<Event>().LoadRecord(row)).ToList();

                    }

                    if (json.MeterIDList != null)
                    {
                        object[] ids = json.MeterIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Event WHERE StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND MeterID IN ({param})", ids).Select().Concat(table);
                    }
                    if (json.MeterAssetKeyList != null)
                    {
                        object[] ids = json.MeterAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Event WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.LineIDList != null)
                    {
                        object[] ids = json.LineIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Event WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND LineID IN ({param})", ids).Select().Concat(table);
                    }
                    if (json.LineAssetKeyList != null)
                    {
                        object[] ids = json.LineAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Event WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND LineID IN (SELECT ID FROM Line WHERE AssetKey IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.EventIDList != null)
                    {
                        object[] ids = json.EventIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Event WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND ID IN ({param})", ids).Select().Concat(table);

                    }


                    return table.Select(row => dataContext.Table<Event>().LoadRecord(row)).DistinctBy(evt => evt.ID).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [HttpPost]
        public IEnumerable<Fault> GetFaults(EventJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                DateTime startTime = (json != null ? (json.StartDate ?? DateTime.Parse("01/01/01")) : DateTime.Parse("01/01/01"));
                DateTime endTime = (json != null ? (json.EndDate ?? DateTime.Now) : DateTime.Now);

                IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();
                try
                {
                    if (json == null || (json.EventIDList == null && json.MeterAssetKeyList == null && json.LineIDList == null && json.LineAssetKeyList == null && json.MeterIDList == null))
                    {
                        table = dataContext.Connection.RetrieveData("Select * FROM FaultSummary WHERE Inception >= {0} AND Inception <= {1}", startTime, endTime).Select();
                        return table.Select(row => dataContext.Table<Fault>().LoadRecord(row)).ToList();

                    }


                    if (json.MeterIDList != null)
                    {
                        object[] ids = json.MeterIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM FaultSummary WHERE Inception >= '{startTime}' AND Inception <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.MeterAssetKeyList != null)
                    {
                        object[] ids = json.MeterAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM FaultSummary WHERE  Inception >= '{startTime}' AND Inception <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.LineIDList != null)
                    {
                        object[] ids = json.LineIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM FaultSummary WHERE  Inception >= '{startTime}' AND Inception <= '{endTime}' AND  EventID IN (Select ID FROM Event WHERE LineID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.LineAssetKeyList != null)
                    {
                        object[] ids = json.LineAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM FaultSummary WHERE  Inception >= '{startTime}' AND Inception <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE LineID IN (SELECT ID FROM Line WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.EventIDList != null)
                    {
                        object[] ids = json.EventIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM FaultSummary WHERE  Inception >= '{startTime}' AND Inception <= '{endTime}' AND EventID IN ({param})", ids).Select().Concat(table);

                    }


                    return table.Select(row => dataContext.Table<Fault>().LoadRecord(row)).DistinctBy(evt => evt.ID).ToList();
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        [HttpPost]
        public IEnumerable<Disturbance> GetDisturbances(EventJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                DateTime startTime = (json != null ? (json.StartDate ?? DateTime.Parse("01/01/01")) : DateTime.Parse("01/01/01"));
                DateTime endTime = (json != null ? (json.EndDate ?? DateTime.Now) : DateTime.Now);

                IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();
                try
                {
                    if (json == null || (json.EventIDList == null && json.MeterAssetKeyList == null && json.LineIDList == null && json.LineAssetKeyList == null && json.MeterIDList == null))
                    {
                        table = dataContext.Connection.RetrieveData("Select * FROM Disturbance WHERE StartTime >= {0} AND EndTime <= {1}", startTime, endTime).Select();
                        return table.Select(row => dataContext.Table<Disturbance>().LoadRecord(row)).ToList();

                    }


                    if (json.MeterIDList != null)
                    {
                        object[] ids = json.MeterIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Disturbance WHERE StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.MeterAssetKeyList != null)
                    {
                        object[] ids = json.MeterAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Disturbance WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.LineIDList != null)
                    {
                        object[] ids = json.LineIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Disturbance WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND  EventID IN (Select ID FROM Event WHERE LineID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.LineAssetKeyList != null)
                    {
                        object[] ids = json.LineAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Disturbance WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE LineID IN (SELECT ID FROM Line WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.EventIDList != null)
                    {
                        object[] ids = json.EventIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM Disturbance WHERE  StartTime >= '{startTime}' AND EndTime <= '{endTime}' AND EventID IN ({param})", ids).Select().Concat(table);

                    }


                    return table.Select(row => dataContext.Table<Disturbance>().LoadRecord(row)).DistinctBy(evt => evt.ID).ToList();
                }
                catch (Exception)
                {
                    
                   return null;
                }
            }
        }

        [HttpPost]
        public IEnumerable<BreakerOperation> GetBreakerOperations(EventJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                DateTime startTime = (json != null ? (json.StartDate ?? DateTime.Parse("01/01/01")) : DateTime.Parse("01/01/01"));
                DateTime endTime = (json != null ? (json.EndDate ?? DateTime.Now) : DateTime.Now);

                IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();
                try
                {
                    if (json == null || (json.EventIDList == null && json.MeterAssetKeyList == null && json.LineIDList == null && json.LineAssetKeyList == null && json.MeterIDList == null))
                    {
                        table = dataContext.Connection.RetrieveData("Select * FROM BreakerOperation WHERE TripCoilEnergized >= {0} AND TripCoilEnergized <= {1}", startTime, endTime).Select();
                        return table.Select(row => dataContext.Table<BreakerOperation>().LoadRecord(row)).ToList();

                    }


                    if (json.MeterIDList != null)
                    {
                        object[] ids = json.MeterIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM BreakerOperation WHERE TripCoilEnergized >= '{startTime}' AND TripCoilEnergized <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.MeterAssetKeyList != null)
                    {
                        object[] ids = json.MeterAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM BreakerOperation WHERE  TripCoilEnergized >= '{startTime}' AND TripCoilEnergized <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.LineIDList != null)
                    {
                        object[] ids = json.LineIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM BreakerOperation WHERE  TripCoilEnergized >= '{startTime}' AND TripCoilEnergized <= '{endTime}' AND  EventID IN (Select ID FROM Event WHERE LineID IN ({param}))", ids).Select().Concat(table);
                    }
                    if (json.LineAssetKeyList != null)
                    {
                        object[] ids = json.LineAssetKeyList.Split(',').Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM BreakerOperation WHERE  TripCoilEnergized >= '{startTime}' AND TripCoilEnergized <= '{endTime}' AND EventID IN (Select ID FROM Event WHERE LineID IN (SELECT ID FROM Line WHERE AssetKey IN ({param})))", ids).Select().Concat(table);
                    }
                    if (json.EventIDList != null)
                    {
                        object[] ids = json.EventIDList.Split(',').Select(int.Parse).Cast<object>().ToArray();
                        string param = string.Join(",", ids.Select((id, index) => $"{{{index}}}"));
                        table = dataContext.Connection.RetrieveData($"Select * FROM BreakerOperation WHERE  TripCoilEnergized >= '{startTime}' AND TripCoilEnergized <= '{endTime}' AND EventID IN ({param})", ids).Select().Concat(table);

                    }


                    return table.Select(row => dataContext.Table<BreakerOperation>().LoadRecord(row)).DistinctBy(evt => evt.ID).ToList();
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        #endregion

        #region [Event Data Calls]

        [HttpPost]
        public IEnumerable<EventData> GetEventWaveformData(EventDataJSON json)
        {
            if(json != null && json.EventID != null)
            {
                try {
                    int eventID = int.Parse(json.EventID);
                    using (DataContext dataContext = new DataContext("systemSettings"))
                    {
                        DataTable table = dataContext.Connection.RetrieveData("Select * FROM GetEventData({0}) as GottenEventData JOIN Series ON GottenEventData.SeriesID = Series.ID JOIN Channel ON Series.ChannelID = Channel.ID WHERE Characteristic = 'Instantaneous'", eventID);
                        return table.Select().Select(row => dataContext.Table<EventData>().LoadRecord(row)).ToList();
                    }
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            else
                return null;
        }

        [HttpPost]
        public IEnumerable<EventData> GetEventFrequencyDomainData(EventDataJSON json)
        {
            if (json != null && json.EventID != null)
            {
                try
                {
                    int eventID = int.Parse(json.EventID);
                    using (DataContext dataContext = new DataContext("systemSettings"))
                    {
                        DataTable table = dataContext.Connection.RetrieveData("Select * FROM GetEventData({0}) as GottenEventData JOIN Series ON GottenEventData.SeriesID = Series.ID JOIN Channel ON Series.ChannelID = Channel.ID WHERE Characteristic <> 'Instantaneous'", eventID);
                        return table.Select().Select(row => dataContext.Table<EventData>().LoadRecord(row)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
                return null;
        }

        #endregion
    }
}