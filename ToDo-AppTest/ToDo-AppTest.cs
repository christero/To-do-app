using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App;

namespace ToDo_AppTest
{
    [TestClass]
    public class UnitTest1
    {
        Repository repository;
        Mock<IFileService> mockFileService;

        [TestInitialize]
        public void SetupContext()
        {
            mockFileService = new Mock<IFileService>();
            repository = new Repository(mockFileService.Object);
        }

        [TestMethod]
        public void Add_ReturnCreatedTodo()
        {
            var result = repository.Add("Well hello there");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ToDo));
            Assert.AreEqual(result.id, 1);
            Assert.AreEqual(result.description, "Well hello there");
        }

        [TestMethod]
        public void Remove_ElementExist_ReturnRemovedTodo()
        {
            repository.Add("Make dinner");
            var removedTodo = repository.Remove(1);

            Assert.IsNotNull(removedTodo);
            Assert.AreEqual("Make dinner", removedTodo.description);
        }

        [TestMethod]
        public void Remove_ElementDoNotExist_ReturnNull()
        {
            var removedTodo = repository.Remove(99);

            Assert.IsNull(removedTodo);
        }

        [TestMethod]
        public void getAll_Returns()
        {
            Dictionary<int, ToDo> todoDict = new Dictionary<int, ToDo>()
            {
                {  1, new ToDo { id = 1, description = "Go school" }},
                {  2, new ToDo { id = 2, description = "Make dinner" }},
                {  3, new ToDo { id = 3, description = "Apply to DIPS" }}
            };
            mockFileService.Setup(x => x.ReadFromFile()).Returns(Task.FromResult(todoDict));

            var dict = repository.GetAll().Result;

            Assert.AreEqual(3, dict.Count, "Got wrong number of Todos");
            int id = 1;
            foreach (var keyValue in dict.OrderBy(x => x.Key))
            {
                Assert.IsTrue(keyValue.Value.id == todoDict[id].id);
                Assert.IsTrue(keyValue.Value.description == todoDict[id++].description);
            }
        }

        [TestMethod]
        public void splitHashtagAndId()
        {
            string[] arguments = { "#1", "#", "1", "#@" };
            int?[] expect = { 1, null, null, null };
            int? result;

            for (int i = 0; i < arguments.Length; i++)
            {
                result = Program.SplitHashtagAndId(arguments[i]);
                if(i == 0)
                Assert.IsTrue((int) result == expect[i]);
                else
                    Assert.IsTrue(result == expect[i]);
            }
        }
    }
}
