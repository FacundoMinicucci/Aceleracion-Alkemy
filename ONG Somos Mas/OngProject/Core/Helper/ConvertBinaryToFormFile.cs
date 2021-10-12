using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Helper
{
    public class ConvertBinaryToFormFile
    {
        public static IFormFile BinaryToFormFile(string image)
        {
            byte[] bytesFile = Convert.FromBase64String(image);
            string fileExtension = ValidateFiles.GetImageExtensionFromFile(bytesFile);
            string uniqueName = "image_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");

            FormFileData formFileData = new()
            {
                FileName = uniqueName + fileExtension,
                ContentType = "Image/" + fileExtension.Replace(".", ""),
                Name = null
            };

            MemoryStream stream = new MemoryStream(bytesFile);
            IFormFile file = new FormFile(stream, 0, bytesFile.Length, formFileData.Name, formFileData.FileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = formFileData.ContentType
            };
            return file;
        }
    }
}
