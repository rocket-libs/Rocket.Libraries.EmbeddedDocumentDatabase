using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase
{
    internal static class ConstraintEnforcer
    {
        public static void EnsureUniqueFieldNames(ImmutableList<RocketDocumentDatabaseField> data)
        {
            if (data == null)
            {
                throw new NullReferenceException($"Data has not been set");
            }
            var duplicatedKeys = data.GroupBy(field => field.Key)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToImmutableList();

            if (duplicatedKeys.Count > 0)
            {
                var errorMessage = "Field keys must be unique. The following key(s) are duplicated";
                foreach (var singleKey in duplicatedKeys)
                {
                    errorMessage += $" '{singleKey}',";
                }
                errorMessage = errorMessage.Substring(0, errorMessage.Length - 1);
                throw new Exception(errorMessage);
            }
        }
    }
}