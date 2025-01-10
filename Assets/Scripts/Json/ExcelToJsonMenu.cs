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
using Unity.VisualScripting;

public class ExcelToJsonMenu
{
    private const string excelFolderPath = "Assets/excel_config";
    private const string jsonFolderPath = "Assets/Resources/Json_config";
    private const string INT_NAME = "int";
    private const string STRING_NAME = "string";
    private const string FLOAT_NAME = "float";

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

        // 获取类型（第二行）
        headerRow = sheet.GetRow(1);
        if (headerRow == null) return tableData;

        List<string> types = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            ICell cell = headerRow.GetCell(i);
            types.Add(cell != null ? cell.StringCellValue : $"Column{i}");
        }

        // 遍历行，从第4行开始（第一行是表头，第二行是类型，第三行是描述）
        for (int i = 3; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;

            ICell idCell = row.GetCell(1);
            if(idCell.NumericCellValue == 0) continue;

            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < headers.Count; j++)
            {
                ICell cell = row.GetCell(j);
                object value = null;

                // 处理单元格内容
                if (cell != null)
                {
                    switch (types[j])
                    {
                        case INT_NAME:
                            value = (int)cell.NumericCellValue;
                            break;
                        case FLOAT_NAME:
                            value = cell.NumericCellValue;
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

public class TypeSelector
{
    public static Type GetTypeFromName(string typeName)
    {
        // 定义类型名称和类型的映射
        var typeMap = new Dictionary<string, Type>
        {
            { "string", typeof(string) },
            { "int", typeof(int) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "bool", typeof(bool) },
            { "char", typeof(char) },
            { "object", typeof(object) },
            { "long", typeof(long) },
            { "short", typeof(short) },
            { "byte", typeof(byte) },
            { "decimal", typeof(decimal) }
        };

        // 尝试从映射中获取类型
        if (typeMap.TryGetValue(typeName, out Type type))
        {
            return type;
        }

        // 如果不在映射中，尝试用 Type.GetType 获取完整类型
        return Type.GetType(typeName);
    }
}