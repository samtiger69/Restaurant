using Restaurant.Entities;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Models
{
    public class Cache
    {
        #region Attributes
        private static List<Branch> Branches;
        private static List<MealType> MealTypes;
        private static List<Meal> Meals;
        private static List<AttributeGroup> AttributeGroups;
        private static List<Entities.Attribute> Attributes;
        private static List<MealAttributeGroup> MealAttributeGroups;
        private static List<MealAttribute> MealAttributes;
        #endregion

        public static void ResetBranches()
        {
            Branches = null;
        }
        public static List<Branch> GetBranches(Request<BaseList> request)
        {
            try
            {
                if (Branches == null)
                {
                    var branchService = BranchService.GetInstance();
                    var response = branchService.List(new Request());
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

        public static void ResetMealTypes()
        {
            MealTypes = null;
        }
        public static List<MealType> GetMealTypes(Request<MealTypeList> request)
        {
            try
            {
                if (MealTypes == null)
                {
                    var mealTypeService = MealTypeService.GetInstance();
                    var response = mealTypeService.List(new Request());
                    MealTypes = response.Data;
                }
                Request<BaseList> baseFilter;
                if (request == null || request.Data == null)
                    baseFilter = new Request<BaseList>
                    {
                        Data = new BaseList()
                    };
                else
                    baseFilter = new Request<BaseList>
                    {
                        Data = request.Data
                    };
                return MealTypes.Where(m => CheckBasicFilter(m, baseFilter) &&
                    (request == null || request.Data == null || !request.Data.BranchId.HasValue || m.BranchId == request.Data.BranchId))
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

        public static void ResetMeals()
        {
            Meals = null;
            MealAttributes = null;
            MealAttributeGroups = null;
        }
        public static List<Meal> GetMeals(Request<MealList> request)
        {
            try
            {
                if (Meals == null)
                {
                    var mealService = MealService.GetInstance();
                    var response = mealService.List(new Request());
                    Meals = response.Data;
                }
                Request<BaseList> baseFilter;
                if (request == null || request.Data == null)
                    baseFilter = new Request<BaseList>
                    {
                        Data = new BaseList()
                    };
                else
                    baseFilter = new Request<BaseList>
                    {
                        Data = request.Data
                    };
                return Meals.Where(m => CheckBasicFilter(m, baseFilter) &&
                    (request == null || request.Data == null || !request.Data.MealTypeId.HasValue || m.MealTypeId == request.Data.MealTypeId))
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

        public static List<MealAttribute> GetMealAttributes(int mealId)
        {
            try
            {
                if (MealAttributes == null)
                {
                    var mealAttributeService = MealAttributeService.GetInstance();
                    var response = mealAttributeService.List(new Request());
                    MealAttributes = response.Data;
                }
                return MealAttributes.Where(m => m.MealId == mealId).ToList();
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

        public static List<MealAttributeGroup> GetMealAttributeGroups(int mealId)
        {
            try
            {
                if (MealAttributeGroups == null)
                {
                    var mealAttributeGroupService = MealAttributeGroupService.GetInstance();
                    var response = mealAttributeGroupService.List(new Request());
                    MealAttributeGroups = response.Data;
                }
                return MealAttributeGroups.Where(m => m.MealId == mealId).ToList();
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

        public static void ResetAttributeGroups()
        {
            AttributeGroups = null;
        }
        public static List<AttributeGroup> GetAttributeGroups(Request<BaseList> request)
        {
            try
            {
                if (AttributeGroups == null)
                {
                    var attributeGroupService = AttributeGroupService.GetInstance();
                    var response = attributeGroupService.List(new Request());
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

        public static void ResetAttributes()
        {
            Attributes = null;
        }
        public static List<Entities.Attribute> GetAttributes(Request<BaseList> request, int? groupId = null)
        {
            try
            {
                if (AttributeGroups == null)
                {
                    var attributeService = AttributeService.GetInstance();
                    var response = attributeService.List(new Request());
                    Attributes = response.Data;
                }
                return Attributes.Where(m => CheckBasicFilter(m,request) && m.GroupId == groupId).ToList();
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

        public static MealInfo GetMealInfo(Request<int> request)
        {
            try
            {
                var mealInfo = new MealInfo();

                var mealAttributes = GetMealAttributes(request.Data);

                var mealAttributeGroups = GetMealAttributeGroups(request.Data);

                foreach (var mealAttributeGroup in mealAttributeGroups)
                {
                    var attributeGroups = GetAttributeGroups(new Request<BaseList> { UserId = request.UserId, Data = new BaseList { Id = request.Data } });
                    foreach (var attributeGroup in attributeGroups)
                    {
                        attributeGroup.Attributes = GetAttributes(new Request<BaseList>(), mealAttributeGroup.AttributeGroupId);
                        mealInfo.AttributeGroups.Add(attributeGroup);
                    }
                }

                foreach (var attribute in mealAttributes)
                {
                    mealInfo.Attributes.Add(GetAttributes(new Request<BaseList>() { UserId = request.UserId, Data = new BaseList { Id = attribute.AttributeId } }).First());
                }
                return mealInfo;
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

        private static bool CheckBasicFilter(BasicEntity entity, Request<BaseList> filter)
        {
            return (filter == null || filter.Data == null) ||
                        ((!filter.Data.Id.HasValue || entity.Id == filter.Data.Id.Value)
                        && (string.IsNullOrEmpty(filter.Data.Name) || entity.Name == filter.Data.Name) &&
                        (string.IsNullOrEmpty(filter.Data.NameAr) || entity.NameAr == filter.Data.NameAr));
        }
    }
}