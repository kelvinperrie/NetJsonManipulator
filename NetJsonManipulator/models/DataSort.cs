using System;
using System.Collections.Generic;
using System.Text;

namespace NetJsonManipulator.models
{
    public class DataSort
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Field { get; set; }
        public string Direction { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }
    }
}
