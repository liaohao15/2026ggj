using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReader
{
    public struct ExcelData
    {
        public string backgroundImageName; // A列：背景图片名
    }

    public static List<ExcelData> ReadExcelFromResources(string resourcePath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // 核心修复：去掉.xlsx后缀，Unity导入后资源名无后缀
        TextAsset excelAsset = Resources.Load<TextAsset>("one/Story/1");
        if (excelAsset == null)
        {
            // 提示语同步修正，方便排查
            Debug.LogError("找不到Excel文件：请检查是否存在 Assets/Resources/one/Story/1.xlsx（Unity内显示为1）");
            return excelData;
        }

        try
        {
            // 二进制流读取Excel（适配打包）
            using (var stream = new MemoryStream(excelAsset.bytes))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        bool isFirstRow = false;
                        while (reader.Read())
                        {
                            if (isFirstRow)
                            {
                                isFirstRow = false;
                                continue;
                            }

                            ExcelData data = new ExcelData();
                            if (reader.FieldCount > 0)
                            {
                                data.backgroundImageName = reader.IsDBNull(0)
                                    ? string.Empty
                                    : reader.GetValue(0).ToString().Trim();
                            }
                            else
                            {
                                Debug.LogWarning("Excel行无有效列数据，跳过");
                                continue;
                            }

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

        if (excelData.Count == 0)
        {
            Debug.LogWarning("Excel无有效数据行：one/Story/1");
        }
        else
        {
            Debug.Log("成功读取Excel数据行数：" + excelData.Count);
        }

        return excelData;
    }
}