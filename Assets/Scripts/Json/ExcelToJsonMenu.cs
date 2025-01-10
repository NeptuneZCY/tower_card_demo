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
using Unity.VisualScripting;

public class ExcelToJsonMenu
{
    private const string excelFolderPath = "Assets/excel_config";
    private const string jsonFolderPath = "Assets/Resources/Json_config";
    private const string INT_NAME = "int";
    private const string STRING_NAME = "string";
    private const string FLOAT_NAME = "float";

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

        // ��ȡ���ͣ��ڶ��У�
        headerRow = sheet.GetRow(1);
        if (headerRow == null) return tableData;

        List<string> types = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            ICell cell = headerRow.GetCell(i);
            types.Add(cell != null ? cell.StringCellValue : $"Column{i}");
        }

        // �����У��ӵ�4�п�ʼ����һ���Ǳ�ͷ���ڶ��������ͣ���������������
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

                // ����Ԫ������
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

public class TypeSelector
{
    public static Type GetTypeFromName(string typeName)
    {
        // �����������ƺ����͵�ӳ��
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

        // ���Դ�ӳ���л�ȡ����
        if (typeMap.TryGetValue(typeName, out Type type))
        {
            return type;
        }

        // �������ӳ���У������� Type.GetType ��ȡ��������
        return Type.GetType(typeName);
    }
}