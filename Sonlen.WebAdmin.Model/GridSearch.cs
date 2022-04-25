using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 表格查詢
    /// </summary>
    public class GridSearch
    {
        /// <summary>
        /// 查詢類型(AND || OR)
        /// </summary>
        public string Mode { get; set; } = "AND";

        /// <summary>
        /// 查詢條件
        /// </summary>
        public List<GridSearchContext> Contexts { get; set; } = new List<GridSearchContext>();
    }

    /// <summary>
    /// 表格查詢條件內容
    /// </summary>
    public class GridSearchContext
    {
        /// <summary>
        /// 查詢欄位
        /// </summary>
        public string ColName { get; set; } = string.Empty;

        /// <summary>
        /// 查詢條件
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// 查詢內容
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// 是否是文字類型的資料
        /// </summary>
        public bool IsString { get; set; } = true;
    }

    /// <summary>
    /// 儲存表格資料用以查詢
    /// </summary>
    public class GridDataForSearch
    {
        /// <summary>
        /// 資料索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 欄位值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 欄位值類別
        /// </summary>
        public string TypeName { get; set; } = string.Empty;
    }
}
