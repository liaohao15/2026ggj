using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReader
{
    // 完全还原视频里的字段定义（不调换列）
    public struct ExcelData
    {
        public string speakerName;         // A列：说话人
        public string speakingContent;     // B列：对话内容
        public string avatarImageFileName; // C列：头像文件名（视频原版）
        public string vocalAudioFileName;  // D列：音频文件名（视频原版）
    }

    public static List<ExcelData> ReadExcel(string filePath)
    {
        List<ExcelData> excelData = new List<ExcelData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    bool isFirstRow = true; // 跳过Excel表头
                    while (reader.Read())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        ExcelData data = new ExcelData();
                        // 完全按视频原版读取列（不调换）
                        data.speakerName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString().Trim();
                        data.speakingContent = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1).ToString().Trim();
                        data.avatarImageFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3).ToString().Trim(); // C列=头像
                        data.vocalAudioFileName = reader.IsDBNull(2) ? string.Empty : reader.GetValue(2).ToString().Trim(); // D列=音频

                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }
}