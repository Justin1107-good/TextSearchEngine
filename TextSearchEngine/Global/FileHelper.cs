using Aspose.Html.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using OpenXmlPowerTools;
using SharpCompress.Common;
using System.IO.Compression;

namespace TextSearchEngine.Global
{
    public class FileHelper
    {
        public static string[] getFilesTypeofHtml(string tempPath)
        {
            if (!Directory.Exists(tempPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
                directoryInfo.Create();
            }
            string[] files = Directory.GetFiles(tempPath, "*.html");
            return files;
        }
        public static void ConvertToHtml(ref string fileID, IFormFile formFile, ref string htmlPath, ref string tempPath,string suffix)
        {
            tempPath = System.IO.Path.GetTempPath() + "\\" + "setemp";
            if (!Directory.Exists(tempPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
                directoryInfo.Create();
            }
            fileID = Guid.NewGuid().ToString();
            string fileTempMD = Path.Combine(tempPath, fileID + Path.GetExtension(formFile.FileName));
            using (var stream = new FileStream(fileTempMD, FileMode.CreateNew))
            {
                formFile.CopyTo(stream);
            }
            htmlPath = Path.Combine(tempPath, $"{fileID}.{suffix}");
           
            Converter.ConvertMarkdown(fileTempMD, htmlPath);
        }
        public static void SaveFile(IFormFile formFile, string path1, string tempPath, string fileID, string path2,ref string filename)
        {
            string filePath = Path.Combine(path1, path2);
            string[] file = FileHelper.getFilesTypeofHtml(tempPath);

            // filename = Path.Combine("Files/posts/", Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
             filename = Path.Combine($"{path2}/", fileID + Path.GetExtension(file[0]));
            using (var stream = new FileStream(Path.Combine(path1, filename), FileMode.OpenOrCreate))
            {
                formFile.CopyTo(stream);
                stream.Close();
            }
           
            Directory.Delete(tempPath, true);
        }
    }
}
