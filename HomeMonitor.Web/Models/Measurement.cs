using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomeMonitor.Models
{
    public class MeasurementType
    {
        [Index(Unique = true)]
        public int TypeId { get; set; }
        public string Name { get; set; }
    }

    public class MeasurementBatch
    {
        [AutoIncrement]
        [Index(Unique = true)]
        public int BatchId { get; set; }
        
        [References(typeof(MeasurementType))]
        public int MeasurementType { get; set; }
        
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public DateTime BatchDate { get; set; }

        [Ignore]
        public List<Measurement> Measurements { get; set; } 
    }
    
    public class Measurement
    {
        [AutoIncrement]
        [Index(Unique = true)]
        public int MeasurementId { get; set; }

        [References(typeof(MeasurementBatch))] 
        public int BatchId { get; set; }

        public string SensorId { get; set; }
        public string SensorName { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public DateTime InsertDate { get; set; }
    }

    public class MeasurementReduxStack
    {
        public string MeasurementDate { get; set; }
        public List<MeasurementRedux> Values { get; set; } 
    }

    public class MeasurementRedux
    {
        public string SensorName { get; set; }
        public string Value { get; set; }
    }
}