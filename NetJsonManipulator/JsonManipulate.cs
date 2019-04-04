using NetJsonManipulator.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetJsonManipulator
{
    public static class JsonManipulate
    {

        /// <summary>
        /// applies filters and sort to supplied json data
        /// </summary>
        /// <param name="data">the json data</param>
        /// <param name="dataFilters">collection of filters to apply to data</param>
        /// <param name="dataSorts">collection of sorts to apply to data</param>
        /// <returns></returns>
        public static string ApplyManipulationsToData(string data, List<DataFilter> dataFilters, List<DataSort> dataSorts)
        {
            JObject jdata;
            try
            {
                jdata = JObject.Parse(data);
            }
            catch (Exception)
            {
                // if parsing the data fails then log it, otherwise it is impossible to know what is going on
                //_logger.Info("The following data caused an error (details of error below) when being parsed into a JObject");
                //_logger.Info(data);
                throw;
            }
            foreach (var filter in dataFilters.Where(f => f.Active))
            {
                jdata = ApplyFilterToData(jdata, filter);
            }
            if (dataSorts.Any(s => s.Active))
            {
                ApplySortsToData(jdata, dataSorts.Where(s => s.Active).ToList());
            }
            return JsonConvert.SerializeObject(jdata);
        }

        private static JObject ApplySortsToData(JObject data, List<DataSort> dataSorts)
        {
            // get the collection we want to sort in our data
            JArray existing = (JArray)data.SelectToken("Location.Rows");

            // create a custom comparer that understands how we want sort elements
            var comparer = new JObjectSortComparer(dataSorts);

            // sort the collection using our custom comparer
            var sorted = existing.AsQueryable().OrderBy(obj => (JObject)obj, comparer);

            // put the sorted collection back into the orginal data
            data["Location"]["Rows"] = new JArray(sorted);

            return data;
        }

        private static JObject ApplyFilterToData(JObject data, DataFilter dataFilter)
        {
            JObject jobject = data;

            string filterRoot = dataFilter.FilterRoot; // the node we want to apply the filter at e.g. "Location.Rows";
            string tokenNameToCheck = dataFilter.PropertyNameToFilterOn; // the name of property we want to filter on e.g. "BedId";
            string filterType = dataFilter.FilterType; // the type of filter e.g. "Exclude" or "Include";
            List<string> filterItems = dataFilter.FilterItemValues.Split(',').ToList();  // new List<string> { "K2", "K3" };
            List<JToken> matchedItems = new List<JToken>(); // will contain a list of items that match our filter

            // get the collection we want to apply the filter too
            var rows = jobject.SelectToken(filterRoot);
            // find and make a collection of the items that match our filter
            foreach (var item in rows)
            {
                var value = (string)item.SelectToken(tokenNameToCheck);
                if (filterItems.Contains(value))
                {
                    matchedItems.Add(item);
                }
            }

            // remove any of our matched items
            if (filterType == "Exclude")
            {
                foreach (var item in matchedItems)
                {
                    item.Remove();
                }
            }
            // remove anything not in our matched items (i.e. only keep matches)
            if (filterType == "Include")
            {
                var nonMatches = rows.Where(i => !matchedItems.Contains(i)).ToList();
                foreach (var item in nonMatches)
                {
                    item.Remove();
                }
            }

            return jobject;
        }
    }
}
