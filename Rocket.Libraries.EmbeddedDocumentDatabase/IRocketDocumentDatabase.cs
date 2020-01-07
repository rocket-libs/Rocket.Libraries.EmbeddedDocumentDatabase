﻿using System.Collections.Immutable;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    public interface IRocketDocumentDatabase
    {
        bool FieldExists(string key);

        ImmutableList<RocketDocumentDatabaseField> GetData();

        RocketDocumentDatabaseField GetFieldByKey(string key);

        TValue GetFieldValueByKey<TValue>(string key);

        void InsertField(string key, object value, string datatype = "");

        void SetData(ImmutableList<RocketDocumentDatabaseField> initialData);

        TValue TryGetFieldValueByKey<TValue>(string key);

        void UpdateField(string fieldName, object value);
    }
}