using NetJsonManipulator.models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetJsonManipulator
{
    /// <summary>
    /// compares two jobjects based on a list of sorts (containing a field name and sort direction) passed into the constructor
    /// </summary>
    public class JObjectSortComparer : IComparer<JObject>
    {
        private List<DataSort> requiredSorts;

        public JObjectSortComparer(List<DataSort> sorts)
        {
            requiredSorts = sorts;
        }

        public int Compare(JObject a, JObject b)
        {
            return DoCompare(a, b, 0);
        }

        private int DoCompare(JObject a, JObject b, int depth)
        {
            var fieldName = requiredSorts[depth].Field;

            var compareResult = string.Compare((string)a.SelectToken(fieldName), (string)b.SelectToken(fieldName));
            if (requiredSorts[depth].Direction.ToLower() == "desc")
            {
                compareResult = compareResult * -1;
            }

            if (compareResult == 0 && depth < requiredSorts.Count - 1)
            {
                return DoCompare(a, b, depth + 1);
            }

            return compareResult;
        }
    }
}
