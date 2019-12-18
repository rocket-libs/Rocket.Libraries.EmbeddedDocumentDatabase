using Rocket.Libraries.EmbeddedDocumentDatabase.ValueParsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    internal class Reader : IDisposable
    {
        public RocketDocumentDatabaseField GetFieldByKey(string key, object locker)
        {
            lock (locker)
            {
                var target = RocketDocumentDatabase.Data.SingleOrDefault(kv => kv.Key.Equals(key, StringComparison.InvariantCulture));
                if (target == null)
                {
                    throw new Exception($"Could not find form value description with key '{key}'");
                }
                return target;
            }
        }

        public bool FieldExists(string key, object locker)
        {
            lock (locker)
            {
                return RocketDocumentDatabase.Data.Any(a => a.Key.Equals(key, StringComparison.InvariantCulture));
            }
        }

        public TValue GetFieldValueByKey<TValue>(string key, object locker)
        {
            var formValueDescription = GetFieldByKey(key, locker);
            if (formValueDescription == null)
            {
                return default;
            }
            else
            {
                return ValueParser.Parse<TValue>(formValueDescription.Value);
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FormValueDescriptionFetcher()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}