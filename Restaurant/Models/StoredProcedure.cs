﻿namespace Restaurant.Models
{
    public static class StoredProcedure
    {

        #region Branch
        public const string BRANCH_SELECT = "Branch_Select";
        public const string BRANCH_CREATE = "Branch_Create";
        public const string BRANCH_UPDATE = "Branch_Update";
        #endregion

        #region MealType
        public const string MEAL_TYPE_SELECT = "MealType_Select";
        public const string MEAL_TYPE_CREATE = "MealType_Create";
        public const string MEAL_TYPE_UPDATE = "MealType_Update";
        #endregion

        #region Meal
        public const string MEAL_SELECT = "Meal_Select";
        public const string MEAL_CREATE = "Meal_Create";
        public const string MEAL_UPDATE = "Meal_Update";
        #endregion

        #region Attribute
        public const string ATTRIBUTE_SELECT = "Attribute_Select";
        public const string ATTRIBUTE_CREATE = "Attribute_Create";
        public const string ATTRIBUTE_UPDATE = "Attribute_Update";
        #endregion

        #region AttributeGroup
        public const string ATTRIBUTE_GROUP_SELECT = "AttributeGroup_Select";
        public const string ATTRIBUTE_GROUP_CREATE = "AttributeGroup_Create";
        public const string ATTRIBUTE_GROUP_UPDATE = "AttributeGroup_Update";
        #endregion

        #region Order
        public const string ORDER_SELECT = "Order_Select";
        public const string ORDER_CREATE = "Order_Create";
        public const string ORDER_UPDATE = "Order_Update";
        #endregion

        #region OrderMeal
        public const string ORDER_MEAL_SELECT = "OrderMeal_Select";
        public const string ORDER_MEAL_CREATE = "OrderMeal_Create";
        public const string ORDER_MEAL_UPDATE = "OrderMeal_Update";
        #endregion

        #region OrderMealAttribute
        public const string ORDER_MEAL_ATTRIBUTE_SELECT = "OrderMealAttribute_Select";
        public const string ORDER_MEAL_ATTRIBUTE_CREATE = "OrderMealAttribute_Create";
        public const string ORDER_MEAL_ATTRIBUTE_UPDATE = "OrderMealAttribute_Update";
        #endregion

        #region OrderAddress
        public const string ORDER_ADDRESS_SELECT = "OrderAddress_Select";
        public const string ORDER_ADDRESS_CREATE = "OrderAddress_Create";
        public const string ORDER_ADDRESS_UPDATE = "OrderAddress_Update";
        #endregion
    }
}