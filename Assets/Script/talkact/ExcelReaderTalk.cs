using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReaderTalk
{
    // 仅保留背景图片名字段（A列）
    public struct ExcelDataTalk
    {
        public string backgroundImageName; // A列：背景图片名（如bg1.jpg）
    }

    // 读取Excel：仅读A列，从0行（首行）开始
    public static List<ExcelDataTalk> ReadExcel(string filePath)
    {
        List<ExcelDataTalk> excelData = new List<ExcelDataTalk>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    // 不跳过首行（从0行开始读）
                    bool isFirstRow = false;
                    while (reader.Read())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        ExcelDataTalk data = new ExcelDataTalk();
                        // 仅读A列（索引0）：背景图片名
                        data.backgroundImageName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString().Trim();

                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }
}