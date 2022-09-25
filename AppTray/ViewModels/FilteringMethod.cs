using AppTray.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AppTray.ViewModels
{
    public static class FilteringMethod
    {
        /// <summary>
        /// 候補に表示する最大数
        /// </summary>
        private const int MAX_DISPHITCOUNT = 5;
        /// <summary>
        /// 前回の検索文字列
        /// </summary>
        private static string _searchText = string.Empty;
        /// <summary>
        /// 現在の表示する
        /// </summary>
        private static int _dispHitCount = 0;

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Run(string searchText, object obj)
        {
            if (searchText != _searchText)
            {
                _searchText = searchText;
                _dispHitCount = 0;
            }
            if (_dispHitCount >= MAX_DISPHITCOUNT)
            {
                return false;
            }
            CompareInfo ci = CultureInfo.CurrentCulture.CompareInfo;
            if (ci.IndexOf((obj as AppInfo).AppDisplayName, searchText, CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType) == 0)
            {
                _dispHitCount++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
