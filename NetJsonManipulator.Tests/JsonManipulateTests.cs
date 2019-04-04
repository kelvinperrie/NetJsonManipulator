using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetJsonManipulator.models;
using System.Collections.Generic;

namespace NetJsonManipulator.Tests
{
    [TestClass]
    public class JsonManipulateTests
    {

        // no filters or sorts should mean data comes through as is (but with newtonsoft serialization
        [TestMethod]
        public void ApplyManipulationsToData_NoManipulations_NoDataChanges()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\"}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\"}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\"}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = rawData;

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData,new List<DataFilter>(), new List<DataSort>());

            // assert
            Assert.AreEqual(expectedData, data);
        }


        #region ApplySortToData

        // test a basic sort on bed number (bedid)
        [TestMethod]
        public void ApplySortToData_DataUnorderedBedIdSort_DataSorted()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\"}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\"}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\"}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row3 + "," + row1 + "," + row2 + "]}}";

            var DataSorts = new List<DataSort>
                        {
                            new DataSort{ Active = true, Direction = "asc", Field = "BedId", Order = 1 }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), DataSorts);

            // assert
            Assert.AreEqual(expectedData, data);
        }

        // basic sort on bed number where data already ordered = no change
        [TestMethod]
        public void ApplySortToData_DataOrderedBedIdSort_NoChange()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\"}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\"}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\"}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row3 + "," + row1 + "," + row2 + "]}}";

            var expectedData = rawData;

            var DataSorts = new List<DataSort>
                        {
                            new DataSort{ Active = true, Direction = "asc", Field = "BedId", Order = 1 }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), DataSorts);

            // assert
            Assert.AreEqual(expectedData, data);
        }

        // test that inactive sorts do not apply
        [TestMethod]
        public void ApplySortToData_NoSortsActive_NoChange()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\"}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\"}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\"}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = rawData;

            var DataSorts = new List<DataSort>
                        {
                            new DataSort{ Active = false, Direction = "asc", Field = "BedId", Order = 1 }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), DataSorts);

            // assert
            Assert.AreEqual(expectedData, data);
        }

        // test that descending/ascending sorts work
        [TestMethod]
        public void ApplySortToData_SortDesc_DataSorted()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\"}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\"}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\"}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row2 + "," + row1 + "," + row3 + "]}}";

            var DataSorts = new List<DataSort>
                        {
                            new DataSort{ Active = true, Direction = "desc", Field = "BedId", Order = 1 }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), DataSorts);

            // assert
            Assert.AreEqual(expectedData, data);
        }

        // two sorts in sequence -> data is sorted and order of the sorts is respected
        [TestMethod]
        public void ApplySortToData_MultiSort_DataSorted()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row3 + "," + row1 + "," + row2 + "]}}";

            var DataSorts = new List<DataSort>
                        {
                            new DataSort{ Active = true, Direction = "asc", Field = "BedId", Order = 2 },
                            new DataSort{ Active = true, Direction = "asc", Field = "AdmissionData.Admission.OnLeave", Order = 1 }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), DataSorts);

            // assert
            Assert.AreEqual(expectedData, data);
        }

        #endregion

        #region ApplyFilterToData

        // test no filter = no change in data
        [TestMethod]
        public void ApplyFilterToData_NoFilters_DataUnchanged()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";
            
            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, new List<DataFilter>(), new List<DataSort>());

            // assert
            Assert.AreEqual(rawData, data);
        }
        // test include
        [TestMethod]
        public void ApplyFilterToData_IncludeFilters_RecordsIncluded()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row3 + "]}}";

            var dataFilters = new List<DataFilter>
                        {
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedId", FilterItemValues ="05,02", FilterType = "Include" }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, dataFilters, new List<DataSort>());

            // assert
            Assert.AreEqual(expectedData, data);
        }
        // test exclude
        [TestMethod]
        public void ApplyFilterToData_ExcludeFilters_RecordsExcluded()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row2 + "," + row3 + "]}}";

            var dataFilters = new List<DataFilter>
                        {
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedId", FilterItemValues ="05", FilterType = "Exclude" }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, dataFilters, new List<DataSort>());

            // assert
            Assert.AreEqual(expectedData, data);
        }
        // test inactive filters not run
        [TestMethod]
        public void ApplyFilterToData_NoActiveFilters_DataUnchanged()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            //var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var dataFilters = new List<DataFilter>
                        {
                            new DataFilter { Active = false, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedId", FilterItemValues ="05", FilterType = "Exclude" }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, dataFilters, new List<DataSort>());

            // assert
            Assert.AreEqual(rawData, data);
        }
        // test multi filters
        [TestMethod]
        public void AApplyFilterToData_MultiFilters_DataFiltered()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row2 + "]}}";

            var dataFilters = new List<DataFilter>
                        {
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedId", FilterItemValues ="05", FilterType = "Exclude" },
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedDescription", FilterItemValues ="Nikau Rm 2", FilterType = "Exclude" }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, dataFilters, new List<DataSort>());

            // assert
            Assert.AreEqual(expectedData, data);
        }
        // test multi filters with an include/exclude combination
        [TestMethod]
        public void ApplyFilterToData_MultiFiltersCombo_DataFiltered()
        {
            // arrange
            var row1 = "{\"MixrData\":{\"BedReserved\":\"gary\"},\"BedDescription\":\"Nikau Rm 5\",\"BedId\":\"05\",\"KeyValueGroup\":\"TPW_05\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"6719253\",\"Admission\":{\"OnLeave\":\"true\"}}}";
            var row2 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 10\",\"BedId\":\"10\",\"KeyValueGroup\":\"TPW_10\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"1111111\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var row3 = "{\"MixrData\":{},\"BedDescription\":\"Nikau Rm 2\",\"BedId\":\"02\",\"KeyValueGroup\":\"TPW_02\",\"AdmissionData\":{\"MixrData\":{},\"KeyValueGroup\":\"222222\",\"Admission\":{\"OnLeave\":\"false\"}}}";
            var rawData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "," + row2 + "," + row3 + "]}}";

            var expectedData = "{\"Location\":{\"KeyValueGroup\":\"TPW\",\"Rows\":[" + row1 + "]}}";

            var dataFilters = new List<DataFilter>
                        {
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedId", FilterItemValues ="05,02", FilterType = "Include" },
                            new DataFilter { Active = true, FilterRoot = "Location.Rows", PropertyNameToFilterOn = "BedDescription", FilterItemValues ="Nikau Rm 2", FilterType = "Exclude" }
                        };

            // act
            var data = JsonManipulate.ApplyManipulationsToData(rawData, dataFilters, new List<DataSort>());

            // assert
            Assert.AreEqual(expectedData, data);
        }

        #endregion
    }
}
