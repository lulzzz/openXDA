﻿//******************************************************************************************************
//  Event.cs - Gbtc
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("Event")]
    public class Event
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        public int MeterID { get; set; }

        public int LineID { get; set; }

        public int EventTypeID { get; set; }

        public int EventDataID { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ShortName { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime StartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime EndTime { get; set; }

        public int Samples { get; set; }

        public int TimeZoneOffset { get; set; }

        public int SamplesPerSecond { get; set; }

        public int SamplesPerCycle { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }
    }

    [TableName("EventView")]
    public class EventForDate : EventView { }

    [TableName("EventView")]
    public class EventForDay : EventView { }

    [TableName("EventView")]
    public class EventForMeter : EventView { }

    [TableName("EventView")]
    public class SingleEvent : EventView { }

    [TableName("EventView")]
    public class EventView
    {
        [Searchable(SearchType.LikeExpression)]
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        public int MeterID { get; set; }

        public int LineID { get; set; }

        public int EventTypeID { get; set; }

        public int EventDataID { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ShortName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Samples { get; set; }

        public int TimeZoneOffset { get; set; }

        public int SamplesPerSecond { get; set; }

        public int SamplesPerCycle { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }

        [Searchable]
        public string LineName { get; set; }

        [Searchable]
        public string MeterName { get; set; }

        public string StationName { get; set; }

        public double Length { get; set; }

        [Searchable]
        public string EventTypeName { get; set; }
    }

    public class MeterEventsByLine
    {
        public int thelineid { get; set; }

        public int theeventid { get; set; }

        public string theeventtype { get; set; }

        public DateTime theinceptiontime { get; set; }

        public string thelinename { get; set; }

        public int voltage { get; set; }

        public string thefaulttype { get; set; }

        public string thecurrentdistance { get; set; }

        public bool pqiexists { get; set; }
    }
}