using System.Collections.Immutable;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    public interface IRocketDocumentDatabase
    {
        ImmutableList<RocketDocumentDatabaseField> GetData();

        RocketDocumentDatabaseField GetFieldByKey(string key);

        TValue GetFieldValueByKey<TValue>(string key);

        void InsertField(string key, object value, string datatype = "");

        void SetData(ImmutableList<RocketDocumentDatabaseField> initialData);

        void UpdateField(string fieldName, object value);
    }
}