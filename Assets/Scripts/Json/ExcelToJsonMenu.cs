using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // 用于读取 .xlsx 文件
using NPOI.HSSF.UserModel; // 用于读取 .xls 文件
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System;
using LitJson;

public class ExcelToJsonGeneric
{
    private static string excelFolderPath = "Assets/excel_config";
    private static string jsonFolderPath = "Assets/Resources/Json_config";

    [MenuItem("导表工具/批量导表")]
    public static void ConvertAllExcelToJson()
    {
        if (!Directory.Exists(excelFolderPath))
        {
            Debug.LogError($"Excel folder does not exist: {excelFolderPath}");
            return;
        }

        // 递归遍历 Excel 文件夹
        ProcessDirectory(excelFolderPath);

        Debug.Log("Excel files converted to JSON successfully.");
        AssetDatabase.Refresh(); // 自动刷新 Unity 资源
    }

    // 递归处理文件夹
    private static void ProcessDirectory(string currentPath)
    {
        // 处理当前目录中的所有 Excel 文件
        string[] excelFiles = Directory.GetFiles(currentPath, "*.xls*", SearchOption.TopDirectoryOnly)
            .Where(file => (file.EndsWith(".xls") ||
                            file.EndsWith(".xlsx")) &&
                            !Path.GetFileName(file).StartsWith("~"))
            .ToArray();

        foreach (var excelFile in excelFiles)
        {
            Debug.Log($"Reading {excelFile}...");
            string relativePath = GetRelativePath(excelFile, excelFolderPath);
            string jsonFilePath = Path.Combine(jsonFolderPath, relativePath);
            jsonFilePath = Path.ChangeExtension(jsonFilePath, ".json"); // 替换扩展名为 .json

            // 确保目标文件夹存在
            Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));

            // 转换 Excel 为 JSON
            List<Dictionary<string, object>> tableData = ReadExcel(excelFile);
            List<string> rows = new List<string>();
            string json = JsonMapper.ToJson(tableData);
            Debug.Log($"Content: {json}");
            WriteToFile(jsonFilePath, json);
        }

        // 递归处理子目录
        string[] subDirectories = Directory.GetDirectories(currentPath);
        foreach (var subDir in subDirectories)
        {
            ProcessDirectory(subDir);
        }
    }

    // 读取 Excel 文件
    private static List<Dictionary<string, object>> ReadExcel(string filePath)
    {
        List<Dictionary<string, object>> tableData = new List<Dictionary<string, object>>();

        // 确定 Excel 文件格式
        IWorkbook workbook;
        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            if (filePath.EndsWith(".xls"))
                workbook = new HSSFWorkbook(file); // .xls
            else
                workbook = new XSSFWorkbook(file); // .xlsx
        }

        // 获取第一个 Sheet
        ISheet sheet = workbook.GetSheetAt(0);

        // 获取表头（第一行）
        IRow headerRow = sheet.GetRow(0);
        if (headerRow == null) return tableData;

        List<string> headers = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            ICell cell = headerRow.GetCell(i);
            headers.Add(cell != null ? cell.StringCellValue : $"Column{i}");
        }

        // 遍历行，从第二行开始（第一行是表头）
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;

            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < headers.Count; j++)
            {
                ICell cell = row.GetCell(j);
                object value = null;

                // 处理单元格内容
                if (cell != null)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            value = cell.NumericCellValue;
                            break;
                        case CellType.String:
                            value = cell.StringCellValue;
                            break;
                        case CellType.Boolean:
                            value = cell.BooleanCellValue;
                            break;
                        default:
                            value = cell.ToString();
                            break;
                    }
                }
                rowData[headers[j]] = value;
            }

            tableData.Add(rowData);
        }

        return tableData;
    }

    // 写入 JSON 文件
    private static void WriteToFile(string path, string content)
    {
        File.WriteAllText(path, content, Encoding.UTF8);
        Debug.Log($"JSON written to: {path}");
    }

    // 获取相对路径
    private static string GetRelativePath(string fullPath, string rootPath)
    {
        return fullPath.Substring(rootPath.Length + 1).Replace("\\", "/");
    }
}

// 用于序列化列表为 JSON
//[System.Serializable]
//public class Serialization<T>
//{
//    [SerializeField]
//    private List<T> items;

//    public Serialization(List<T> items)
//    {
//        this.items = items;
//    }

//    public List<T> ToList()
//    {
//        return items;
//    }
//}

[Serializable]
public class JsonDictionary<K, V> : ISerializable
{
    Dictionary<K, V> dict = new Dictionary<K, V>();

    public JsonDictionary() { }

    protected JsonDictionary(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        foreach (K key in dict.Keys)
        {
            info.AddValue(key.ToString(), dict[key]);
        }
    }

    public void Add(K key, V value)
    {
        dict.Add(key, value);
    }

    public V this[K index]
    {
        set { dict[index] = value; }
        get { return dict[index]; }
    }
}