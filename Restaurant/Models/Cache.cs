using Restaurant.Entities;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Cache
    {
        private static List<Branch> Branches;
        private static List<MealType> MealTypes;
        private static List<Meal> Meals;
        private static List<AttributeGroup> AttributeGroups;
        private static List<Entities.Attribute> Attributes;

        public static async Task<List<Branch>> GetBranches(BaseListModel request)
        {
            try
            {
                if (Branches == null)
                {
                    var branchService = new BranchService();
                    var response = await branchService.List(new Request());
                    Branches = response.Data;
                }
                return Branches.Where(m => CheckBasicFilter(m, request)).ToList();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<List<MealType>> GetMealTypes(MealTypeListModel request)
        {
            try
            {
                if (MealTypes == null)
                {
                    var mealTypeService = new MealTypeService();
                    var response = await mealTypeService.List(new Request());
                    MealTypes = response.Data;
                }
                return MealTypes.Where(m => CheckBasicFilter(m, request) &&
                    (request == null || !request.BranchId.HasValue || m.BranchId == request.BranchId))
                    .ToList();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<List<Meal>> GetMeals(MealListModel request)
        {
            try
            {
                if (Meals == null)
                {
                    var mealService = new MealService();
                    var response = await mealService.List(new Request());
                    Meals = response.Data;
                }
                return Meals.Where(m => CheckBasicFilter(m, request) &&
                    (request == null || !request.MealTypeId.HasValue || m.MealTypeId == request.MealTypeId))
                    .ToList();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<List<AttributeGroup>> GetAttributeGroups(BaseListModel request)
        {
            try
            {
                if (AttributeGroups == null)
                {
                    var attributeGroupService = new AttributeGroupService();
                    var response = await attributeGroupService.List(new Request());
                    AttributeGroups = response.Data;
                }
                return AttributeGroups.Where(m => CheckBasicFilter(m,request)).ToList();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<List<Entities.Attribute>> GetAttributes(BaseListModel request)
        {
            try
            {
                if (AttributeGroups == null)
                {
                    var attributeService = new AttributeService();
                    var response = await attributeService.List(new Request());
                    Attributes = response.Data;
                }
                return Attributes.Where(m => CheckBasicFilter(m,request)).ToList();
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static bool CheckBasicFilter(BasicEntity entity, BaseListModel filter)
        {
            return (filter == null) ||
                        ((!filter.Id.HasValue || entity.Id == filter.Id.Value)
                        && (string.IsNullOrEmpty(filter.Name) || entity.Name == filter.Name) &&
                        (string.IsNullOrEmpty(filter.NameAr) || entity.NameAr == filter.NameAr));
        }
    }
}