using System;
using System.Collections.Generic;
using System.Text;

namespace NetJsonManipulator.models
{
    public class DataFilter
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string FilterRoot { get; set; }
        public string PropertyNameToFilterOn { get; set; }
        public string FilterItemValues { get; set; }
        public string FilterType { get; set; }
        public bool Active { get; set; }

    }
}
