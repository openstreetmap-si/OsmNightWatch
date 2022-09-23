﻿using LightningDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmNightWatch
{
    public class KeyValueDatabase
    {
        private readonly LightningEnvironment dbEnv;

        private static LightningEnvironment CreateEnv(string storePath)
        {
            var dbEnv = new LightningEnvironment(storePath);
            dbEnv.MaxDatabases = 10;
            dbEnv.MapSize = 16L * 1024L * 1024L * 1024L;//16GB should be enough, I hope
            dbEnv.Open();
            return dbEnv;
        }

        public KeyValueDatabase(string storePath)
        {
            dbEnv = CreateEnv(storePath);
        }

        public LightningTransaction BeginTransaction()
        {
            return dbEnv.BeginTransaction();
        }

        public void SetSequenceNumber(long sequenceNumber, LightningTransaction tx)
        {
            var db = tx.OpenDatabase("ReplicationState", new DatabaseConfiguration() { Flags = DatabaseOpenFlags.Create });
            tx.Put(db, BitConverter.GetBytes(1L), BitConverter.GetBytes(sequenceNumber));
        }

        public long? GetSequenceNumber()
        {
            using (var tx = dbEnv.BeginTransaction())
            using (var db = tx.OpenDatabase("ReplicationState", new DatabaseConfiguration() { Flags = DatabaseOpenFlags.Create }))
            {
                var sequenceNumber = tx.Get(db, BitConverter.GetBytes(1L));
                if (sequenceNumber.resultCode == MDBResultCode.Success)
                {
                    return BitConverter.ToInt64(sequenceNumber.value.AsSpan());
                }
            }
            return null;
        }
    }
}
