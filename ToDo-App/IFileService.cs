using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo_App
{
    public interface IFileService
    {
        void WriteToFile(Dictionary<int, ToDo> todoDictionary);
        Task<Dictionary<int, ToDo>> ReadFromFile();
    }
}
