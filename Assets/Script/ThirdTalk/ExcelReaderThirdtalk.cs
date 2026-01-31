using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReaderThirdtalk
{
    // 仅存储背景图片名（A列）
    public struct ExcelDataThirdtalk
    {
        public string backgroundImageName; // A列：背景图片名（如bg1.jpg，无空格）
    }

    // 读取Excel：仅读A列，从0行（首行）开始
    public static List<ExcelDataThirdtalk> ReadExcel(string filePath)
    {
        List<ExcelDataThirdtalk> excelData = new List<ExcelDataThirdtalk>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // 打开Excel文件流
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

                        ExcelDataThirdtalk data = new ExcelDataThirdtalk();
                        // 读A列（索引0）
                        data.backgroundImageName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString().Trim();

                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }
}