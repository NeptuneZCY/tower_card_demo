using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

public class ExcelExample : MonoBehaviour
{
    // ��ȡ Excel �ļ�
    public List<EventCardData> ReadExcel(string filePath)
    {
        List<EventCardData> cards = new List<EventCardData>();

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

        // �����У��ӵڶ��п�ʼ����һ��ͨ���Ǳ�ͷ��
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