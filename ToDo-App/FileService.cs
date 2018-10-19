using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ToDo_App
{
    public class FileService : IFileService
    {
        Dictionary<int, ToDo> todoDictionary = new Dictionary<int, ToDo>();
        string filepath;

        public FileService(string filepath)
        {
            this.filepath = filepath;

            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
        }

        public async Task<Dictionary<int, ToDo>> ReadFromFile()
        {
            var jsonData = await File.ReadAllTextAsync(filepath);
            if (!string.IsNullOrEmpty(jsonData))
                return JsonConvert.DeserializeObject<Dictionary<int, ToDo>>(jsonData);
            else
                return null;
        }

        public async void WriteToFile(Dictionary<int, ToDo> todoDictionary)
        {
            await File.WriteAllTextAsync(filepath, JsonConvert.SerializeObject(todoDictionary, Formatting.Indented));
        }
    }
}
