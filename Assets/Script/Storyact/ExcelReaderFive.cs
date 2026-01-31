using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using System.Text;
using UnityEngine;

public class ExcelReaderFive
{
    public struct ExcelDataFive
    {
        public string speakerName;         // A列：说话人
        public string speakingContent;     // B列：对话内容
        public string avatarImageFileName; // C列：头像文件名
        public string vocalAudioFileName;  // D列：音频文件名
    }

    public static List<ExcelDataFive> ReadExcel(string filePath)
    {
        List<ExcelDataFive> excelData = new List<ExcelDataFive>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    bool isFirstRow = true;
                    while (reader.Read())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        ExcelDataFive data = new ExcelDataFive();
                        data.speakerName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString().Trim();
                        data.speakingContent = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1).ToString().Trim();
                        data.avatarImageFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3).ToString().Trim();
                        data.vocalAudioFileName = reader.IsDBNull(2) ? string.Empty : reader.GetValue(2).ToString().Trim();

                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }
}