using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HomeMonitor.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace HomeMonitor.Controllers
{
    public class MonitorController : ApiController
    {
        private readonly MeasurementRepository _repo;
        
        public MonitorController()
        {
            _repo = new MeasurementRepository();
        }
        
        
        public HttpResponseMessage Post(MeasurementBatch measurementBatch)
        {
            if (measurementBatch != null && measurementBatch.Measurements != null && measurementBatch.Measurements.Any())
            {
                measurementBatch.BatchDate = measurementBatch.BatchDate.AddSeconds(0 - measurementBatch.BatchDate.Second);
                var batchId = _repo.InsertMeasurementBatch(measurementBatch);

                var reduxStack = new MeasurementReduxStack() { 
                    MeasurementDate = measurementBatch.BatchDate.ToString("H:mm"), 
                    Values = new List<MeasurementRedux>() 
                };
                
                foreach (var measurement in measurementBatch.Measurements)
                {
                    measurement.MeasurementDate = measurement.MeasurementDate.AddSeconds(0-measurement.MeasurementDate.Second);
                    measurement.BatchId = (int)batchId;
                    measurement.InsertDate = DateTime.Now;
                    _repo.InsertMeasurement(measurement);

                    reduxStack.Values.Add(new MeasurementRedux { SensorName = measurement.SensorName, Value = measurement.Value });
                }

                var context = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
                context.Clients.All.MeasurementNotification(JsonConvert.SerializeObject(reduxStack));
            }
            
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
