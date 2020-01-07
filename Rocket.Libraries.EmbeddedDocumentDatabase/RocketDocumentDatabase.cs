using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    public class RocketDocumentDatabase : IDisposable, IRocketDocumentDatabase
    {
        public RocketDocumentDatabase()
        {
            isDisposed = false;
            data = ImmutableList<RocketDocumentDatabaseField>.Empty;
        }

        private static bool isDisposed;

        private readonly object locker = new object();

        internal static ImmutableList<RocketDocumentDatabaseField> Data
        {
            get
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(RocketDocumentDatabase));
                }
                return data;
            }

            set
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(RocketDocumentDatabase));
                }
                data = value;
            }
        }

        public void SetData(ImmutableList<RocketDocumentDatabaseField> initialData)
        {
            ConstraintEnforcer.EnsureUniqueFieldNames(initialData);
            Data = initialData;
        }

        public bool FieldExists(string key)
        {
            using var reader = new Reader();
            return reader.FieldExists(key, locker);
        }

        public ImmutableList<RocketDocumentDatabaseField> GetData()
        {
            lock (locker)
            {
                return Data;
            }
        }

        public void InsertField(string key, object value, string datatype = "")
        {
            using var writer = new Writer();
            writer.InsertField(key, value, locker, datatype: datatype);
        }

        public void UpdateField(string key, object value)
        {
            using var writer = new Writer();
            writer.UpdateField(key, value, locker);
        }

        public RocketDocumentDatabaseField GetFieldByKey(string key)
        {
            using var reader = new Reader();
            return reader.GetFieldByKey(key, locker);
        }

        public TValue GetFieldValueByKey<TValue>(string key)
        {
            using var reader = new Reader();
            return reader.GetFieldValueByKey<TValue>(key, locker);
        }

        public TValue TryGetFieldValueByKey<TValue>(string key)
        {
            using var reader = new Reader();
            return reader.TryGetFieldValueByKey<TValue>(key, locker);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        private static ImmutableList<RocketDocumentDatabaseField> data;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Data = Data.Clear();
                    Data = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RocketDocumentDatabase()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}