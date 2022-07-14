using EmployeesHierarchy;

namespace EmployeesHierarchyTests
{
    [TestClass]
    public class EmployeesTest
    {
        static string inputCsv = "employees.csv";

        Employees employee = new Employees(inputCsv);

        [TestMethod]
        public void TestExtractCsv()
        {
            var data = employee.ExtractCsvData(inputCsv);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestSalaryFieldValid()
        {
            Assert.IsTrue(employee.SalaryFieldIsValid());
        }


        [TestMethod]
        public void TestEmployeesHasNoMoreThanOneManagers()
        {
            Assert.IsFalse(employee.AnyEmployeeWithMoreThanOneManagers());
        }

        [TestMethod]
        public void TestOnlyOneCEO()
        {
            Assert.IsTrue(employee.HasOneCEO());
        }


        [TestMethod]
        public void TestNoCircularReference()
        {
            Assert.IsFalse(employee.HasCircularReference());
        }


        [TestMethod]
        public void TestAllManagersAreEmployees()
        {
            Assert.IsTrue(employee.AllManagersAreEmployees());
        }

        [TestMethod]
        public void TestSalaryBudget()
        {
            long budget = employee.SalaryBudget("Employee1");
            Assert.AreEqual(3800, budget);
        }

    }
}