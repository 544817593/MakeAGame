using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 读取CSV文件相关，之后可能不用CSV存储数据了，但仍然先写着
    /// </summary>
    public interface ICSVImportUtility : IUtility
    {
        /// <summary>
        /// 传入CSV文件路径，读出属性列表、每行数据列表
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="attribute_idx">属性列表</param>
        /// <param name="data_list">数据列表</param>
        void ParseCSV(string resPath, ref Dictionary<string, int> attribute_idx, ref List<List<string>> data_list);
    }
    
    /// <summary>
    /// CSV解析器
    /// </summary>
    public class CSVImportUtility: ICSVImportUtility
    {
        public void ParseCSV(string resPath, ref Dictionary<string, int> attribute_idx, ref List<List<string>> data_list)
        {
            TextAsset table = Resources.Load<TextAsset>(resPath);
            if (table == null)
            {
                Debug.LogError("table is null!");
                return;
            }

            // first line is attribute and remaining lines are data.
            List<string> CSVText = new List<string>(table.text.Split('\n'));
            if (CSVText.Count < 2)
            {
                Debug.LogError("Empty Table or LoadError");
                return;
            }
            List<string> temp_attribute_list = new List<string>(CSVText[0].Split(','));

            // add each attribute into list, skip empty string and '\r'
            for (int i = 0; i < temp_attribute_list.Count; i++)
            {
                if (string.IsNullOrEmpty(temp_attribute_list[i])) continue;
                string attribute = temp_attribute_list[i];
                attribute = attribute.Replace("\r", "");
                attribute_idx.Add(attribute, i);
            }

            CSVText.RemoveAt(0);
            // data_list rows save each card's data.
            for (int i = 0; i < CSVText.Count; i++)
            {
                if (string.IsNullOrEmpty(CSVText[i])) continue;
                string data = CSVText[i];
                data = data.Replace("\r", "");
                List<string> temp_data_list = new List<string>(data.Split(','));
                data_list.Add(temp_data_list);
            }
        }
    }
}