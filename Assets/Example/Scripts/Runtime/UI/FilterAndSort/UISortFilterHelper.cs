using System.Linq;

namespace GameMain.Runtime
{
    public static class UISortFilterHelper
    {
        #region Save
        public static void SaveSortParam(string key, UISortParam sortParam)
        {
            var valKey     = $"{key}{Constant.Setting.SortParamValue}";
            var reverseKey = $"{key}{Constant.Setting.SortIsReverse}";
            
            SettingManager.Instance.SetInt(valKey, sortParam.SortTypeValue);
            SettingManager.Instance.SetBool(reverseKey, sortParam.IsReverseOrder);
        }

        public static UISortParam LoadSortParam(string key, UISortDef def,UISortParam defaultParam)
        {
            var valKey     = $"{key}{Constant.Setting.SortParamValue}";
            var reverseKey = $"{key}{Constant.Setting.SortIsReverse}";
            
            var sortTypeValue = SettingManager.Instance.GetInt(valKey, defaultParam.SortTypeValue);
            var isReverse = SettingManager.Instance.GetBool(reverseKey, defaultParam.IsReverseOrder);
            
            if (ValidateSortParam(def, sortTypeValue))
            {
                defaultParam.ChangeSortType(sortTypeValue);
            }

            defaultParam.SetSortOrder(isReverse);
            defaultParam.Decide();
            return defaultParam;
        }

        private static bool ValidateSortParam(UISortDef def, int sortTypeValue)
        {
            return def.SortCategoryList.Any(_ => _.SortTypeValue == sortTypeValue);
        }
        
        #endregion
    }
}