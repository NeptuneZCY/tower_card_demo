using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

public class ExcelExample : MonoBehaviour
{
    // 读取 Excel 文件
    public List<EventCardData> ReadExcel(string filePath)
    {
        List<EventCardData> cards = new List<EventCardData>();

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

        // 遍历行，从第二行开始（第一行通常是表头）
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;

            EventCardData card = new EventCardData
            {
                id = (int)row.GetCell(0).NumericCellValue,
                name = row.GetCell(1).StringCellValue,
                description = row.GetCell(2).StringCellValue,
                icon = row.GetCell(3).StringCellValue,
                key1_shape = row.GetCell(4).StringCellValue,
                key1_attri = row.GetCell(5).StringCellValue
            };
            cards.Add(card);
        }

        return cards;
    }
}