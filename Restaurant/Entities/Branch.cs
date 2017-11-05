using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Entities
{
    public class Branch : BasicEntity
    {
        public string LocationDescription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<MealType> MealTypes { get; set; }
    }
}