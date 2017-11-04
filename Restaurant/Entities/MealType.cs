using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Entities
{
    public class MealType : BasicEntity
    {
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<Meal> Meals { get; set; }
    }
}