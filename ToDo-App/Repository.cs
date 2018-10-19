using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App
{
    public class Repository
    {
        IFileService fs;
        Dictionary<int, ToDo> todoDictionary;

        public Repository(IFileService fs)
        {
            this.fs = fs;
            todoDictionary = new Dictionary<int, ToDo>();
            Setup();
        }

        public async void Setup()
        {
            var todos = await GetAll();
            if (todos != null)
            {
                todoDictionary = todos;
            }
        }

        public async Task<Dictionary<int, ToDo>> GetAll()
        {
            var result = await fs.ReadFromFile();
            return result;
        }

        public ToDo Add(string task)
        {
            var newTodo = new ToDo()
            {
                id = (todoDictionary.Count != 0) ? todoDictionary.Keys.Max() + 1 : 1,
                description = task
            };
            todoDictionary.Add(newTodo.id, newTodo);
            fs.WriteToFile(todoDictionary);

            return newTodo;
        }

        public ToDo Remove(int id)
        {
            ToDo deletedTodo;

            if (todoDictionary.TryGetValue(id, out deletedTodo))
            {
                todoDictionary.Remove(deletedTodo.id);
                fs.WriteToFile(todoDictionary);
            }

            return deletedTodo;
        }
    }
}
