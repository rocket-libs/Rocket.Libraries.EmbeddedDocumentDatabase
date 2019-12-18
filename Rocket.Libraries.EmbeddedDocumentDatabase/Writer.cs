using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    internal class Writer : IDisposable
    {
        public void InsertField(string key, object value, object locker, string datatype = "")
        {
            using var reader = new Reader();
            var alreadyExists = reader.FieldExists(key, locker);
            if (alreadyExists)
            {
                throw new Exception($"There is already a field with the key '{key}'. Keys must be unique. Did you mean to update?");
            }
            lock (locker)
            {
                RocketDocumentDatabase.Data = RocketDocumentDatabase.Data.Add(new RocketDocumentDatabaseField
                {
                    Key = key,
                    Datatype = datatype,
                });
            }
            UpdateField(key, value, locker);
        }

        public void UpdateField(string key, object value, object locker)
        {
            using var reader = new Reader();
            var field = reader.GetFieldByKey(key, locker);
            lock (locker)
            {
                if (field == default)
                {
                    throw new Exception($"Couldn't find a field called '{key}'");
                }

                field.Value = value == null ? string.Empty : value.ToString();
                RocketDocumentDatabase.Data = RocketDocumentDatabase.Data.Remove(field)
                    .Add(field);
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Writer()
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