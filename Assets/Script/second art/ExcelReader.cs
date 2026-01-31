using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReader
{
    // 仅保留背景图片名字段（A列）
    public struct ExcelData
    {
        public string backgroundImageName; // A列：背景图片名
    }

    public static List<ExcelData> ReadExcel(string filePath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // 1. 校验文件是否存在
        if (!File.Exists(filePath))
        {
            Debug.LogError("Excel文件不存在：" + filePath);
            return excelData;
        }

        try
        {
            // 2. 新增文件共享权限，解决占用冲突
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        // 3. 关键：isFirstRow=false，读取Excel第1行（rbg1.jpg）
                        bool isFirstRow = false;
                        while (reader.Read())
                        {
                            if (isFirstRow)
                            {
                                isFirstRow = false;
                                continue;
                            }

                            ExcelData data = new ExcelData();

                            // 4. 校验列索引，避免越界
                            if (reader.FieldCount > 0)
                            {
                                // 读取A列（索引0）的内容（rbg1.jpg/rbg2.jpg）
                                data.backgroundImageName = reader.IsDBNull(0)
                                    ? string.Empty
                                    : reader.GetValue(0).ToString().Trim();
                            }
                            else
                            {
                                Debug.LogWarning("Excel行无有效列数据，跳过");
                                continue;
                            }

                            // 5. 过滤空数据行
                            if (!string.IsNullOrEmpty(data.backgroundImageName))
                            {
                                excelData.Add(data);
                            }
                        }
                    } while (reader.NextResult());
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取Excel失败：" + e.Message);
            return excelData;
        }

        // 6. 校验数据量
        if (excelData.Count == 0)
        {
            Debug.LogWarning("Excel无有效数据行：" + filePath);
        }
        else
        {
            Debug.Log("成功读取Excel数据行数：" + excelData.Count);
        }

        return excelData;
    }
}