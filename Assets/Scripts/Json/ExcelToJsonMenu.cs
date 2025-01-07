using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // ���ڶ�ȡ .xlsx �ļ�
using NPOI.HSSF.UserModel; // ���ڶ�ȡ .xls �ļ�
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System;
using LitJson;

public class ExcelToJsonGeneric
{
    private static string excelFolderPath = "Assets/excel_config";
    private static string jsonFolderPath = "Assets/Resources/Json_config";

    [MenuItem("������/��������")]
    public static void ConvertAllExcelToJson()
    {
        if (!Directory.Exists(excelFolderPath))
        {
            Debug.LogError($"Excel folder does not exist: {excelFolderPath}");
            return;
        }

        // �ݹ���� Excel �ļ���
        ProcessDirectory(excelFolderPath);

        Debug.Log("Excel files converted to JSON successfully.");
        AssetDatabase.Refresh(); // �Զ�ˢ�� Unity ��Դ
    }

    // �ݹ鴦���ļ���
    private static void ProcessDirectory(string currentPath)
    {
        // ����ǰĿ¼�е����� Excel �ļ�
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
            jsonFilePath = Path.ChangeExtension(jsonFilePath, ".json"); // �滻��չ��Ϊ .json

            // ȷ��Ŀ���ļ��д���
            Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));

            // ת�� Excel Ϊ JSON
            List<Dictionary<string, object>> tableData = ReadExcel(excelFile);
            List<string> rows = new List<string>();
            string json = JsonMapper.ToJson(tableData);
            Debug.Log($"Content: {json}");
            WriteToFile(jsonFilePath, json);
        }

        // �ݹ鴦����Ŀ¼
        string[] subDirectories = Directory.GetDirectories(currentPath);
        foreach (var subDir in subDirectories)
        {
            ProcessDirectory(subDir);
        }
    }

    // ��ȡ Excel �ļ�
    private static List<Dictionary<string, object>> ReadExcel(string filePath)
    {
        List<Dictionary<string, object>> tableData = new List<Dictionary<string, object>>();

        // ȷ�� Excel �ļ���ʽ
        IWorkbook workbook;
        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            if (filePath.EndsWith(".xls"))
                workbook = new HSSFWorkbook(file); // .xls
            else
                workbook = new XSSFWorkbook(file); // .xlsx
        }

        // ��ȡ��һ�� Sheet
        ISheet sheet = workbook.GetSheetAt(0);

        // ��ȡ��ͷ����һ�У�
        IRow headerRow = sheet.GetRow(0);
        if (headerRow == null) return tableData;

        List<string> headers = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            ICell cell = headerRow.GetCell(i);
            headers.Add(cell != null ? cell.StringCellValue : $"Column{i}");
        }

        // �����У��ӵڶ��п�ʼ����һ���Ǳ�ͷ��
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;

            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < headers.Count; j++)
            {
                ICell cell = row.GetCell(j);
                object value = null;

                // ����Ԫ������
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

    // д�� JSON �ļ�
    private static void WriteToFile(string path, string content)
    {
        File.WriteAllText(path, content, Encoding.UTF8);
        Debug.Log($"JSON written to: {path}");
    }

    // ��ȡ���·��
    private static string GetRelativePath(string fullPath, string rootPath)
    {
        return fullPath.Substring(rootPath.Length + 1).Replace("\\", "/");
    }
}

// �������л��б�Ϊ JSON
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