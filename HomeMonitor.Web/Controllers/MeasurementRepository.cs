using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using HomeMonitor.Models;
using ServiceStack.OrmLite;

namespace HomeMonitor.Controllers
{
    public class MeasurementRepository
    {
        private readonly OrmLiteConnectionFactory _dbFactory;
        
        public MeasurementRepository()
        {
            _dbFactory =
               new OrmLiteConnectionFactory(
                   ConfigurationManager.ConnectionStrings["HomeMonitor"].ConnectionString,
                   SqlServerDialect.Provider);
        }

        public void SetupDB()
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<MeasurementType>();
                db.CreateTableIfNotExists<MeasurementBatch>();
                db.CreateTableIfNotExists<Measurement>();

                db.Insert(new MeasurementType {TypeId = 1, Name = "Temperature"});
                db.Insert(new MeasurementType {TypeId = 2, Name = "Movement" });
            }
        }

        public long InsertMeasurementBatch(MeasurementBatch batch)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                db.Insert(batch);
                return db.GetLastInsertId();
            }
        }

        public long InsertMeasurement(Measurement measurement)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                db.Insert(measurement);
                return db.GetLastInsertId();
            }
        }

        public List<MeasurementBatch> Get10MostRecentsBatches(int measurementType)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                var batches =
                    db.Select<MeasurementBatch>(
                        q => q.Where(b => b.MeasurementType == measurementType).OrderByDescending(b => b.BatchDate).Limit(10));

                if (batches != null && batches.Any())
                {
                    foreach (var measurementBatch in batches)
                    {
                        measurementBatch.Measurements = GetMeasurementsByBatchId(measurementBatch.BatchId);
                    }

                    return batches;
                }
            }
            return null;
        }

        public List<Measurement> GetMeasurementsByBatchId(int batchId)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Select<Measurement>(q => q.BatchId == batchId);
            }
        }

    }
}