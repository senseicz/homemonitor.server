using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeMonitor.Models
{
    public class TemperatureViewModel
    {
        private List<string> _sensorNames; 
        
        public List<MeasurementBatch> Batches { get; set; } 

        public List<string> SensorNames
        {
            get { return _sensorNames ?? (_sensorNames = GetSensorNames()); }
        } 
        
        public string Sensors
        {
            get 
            { 
                var sensorNames = GetSensorNames();
                if (sensorNames.Any())
                {
                    var retString = sensorNames.Aggregate("", (current, sensorName) => current + string.Format("'{0}',", sensorName));
                    return retString.TrimEnd(new[] {','});
                }

                return "";
            }
        }

        public Dictionary<string, List<string>> TimeMeasurements
        {
            get
            {
                var retDict = new Dictionary<string, List<string>>();
                if (Batches != null && Batches.Any())
                {
                    foreach (var measurementBatch in Batches)
                    {
                        var key = measurementBatch.BatchDate.ToString("H:mm");
                        
                        if (measurementBatch.Measurements != null && measurementBatch.Measurements.Any())
                        {
                            var values = new List<string>();
                            
                            foreach (var sensorName in SensorNames)
                            {
                                var sensorMeasurement = measurementBatch.Measurements.FirstOrDefault(m => m.SensorName == sensorName);
                                
                                string sensorValue = "NaN";

                                if (sensorMeasurement != null)
                                {
                                    sensorValue = sensorMeasurement.Value.Replace(",", ".");
                                }

                                values.Add(sensorValue);
                            }
                            retDict.Add(key, values);
                        }
                    }
                }
                return retDict;
            }
        }

        private List<string> GetSensorNames()
        {
            var sensorNames = new List<string>();

            if (Batches != null && Batches.Any())
            {
                foreach (var measurementBatch in Batches)
                {
                    if (measurementBatch.Measurements != null && measurementBatch.Measurements.Any())
                    {
                        foreach (var measurement in measurementBatch.Measurements)
                        {
                            if (!sensorNames.Contains(measurement.SensorName))
                            {
                                sensorNames.Add(measurement.SensorName);
                            }
                        }
                    }
                }
                return sensorNames;
            }

            return new List<string>();
        }
    }
}