using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Entities
{
    public class MealType : BasicEntity
    {
        public string Description { get; set; }
        public int BranchId { get; set; }
    }
}