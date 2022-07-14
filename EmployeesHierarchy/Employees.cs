namespace EmployeesHierarchy
{

    public class Employees
    {
        private List<EmployeeDto> staffList = new List<EmployeeDto>();

        public Employees(string employeesFile)
        {
            staffList = ExtractCsvData(employeesFile);

            if (staffList==null
                || !SalaryFieldIsValid()
                || AnyEmployeeWithMoreThanOneManagers()
                || !HasOneCEO()
                || HasCircularReference()
                || !AllManagersAreEmployees())
            {
                throw new Exception("Validation Failed for the provided CSV file!");
            }
        }

        public List<EmployeeDto> ExtractCsvData(string file)
        {
            List<EmployeeDto> result = new List<EmployeeDto>();
            string data = File.ReadAllText(file);
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            else
            {
                try
                {
                    result = data.Split(Environment.NewLine.ToCharArray())
                            .Where(s => !string.IsNullOrEmpty(s))
                            .Select(s => new EmployeeDto
                            {
                                EmployeeId = s.Split(',')[0],
                                ManagerId = s.Split(',')[1],
                                Salary = s.Split(',')[2]
                            }).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool SalaryFieldIsValid()
        {
            try
            {
                int temp;
                bool anyInvalid = staffList.Any(s => !int.TryParse(s.Salary, out temp));
                return !anyInvalid;
            }
            catch
            {
                return false;
            }
        }

        public bool AnyEmployeeWithMoreThanOneManagers()
        {
            return staffList.Any(staff => (staffList.Any(other => other.EmployeeId == staff.EmployeeId && other.ManagerId != staff.ManagerId)));
        }

        public bool HasOneCEO()
        {
            List<EmployeeDto> ceos = staffList.Where(s => string.IsNullOrEmpty(s.ManagerId)).ToList();
            return ceos!=null && ceos.Count() == 1;
        }

        public bool HasCircularReference()
        {
            return staffList.Any(employee => (staffList.Any(manager => employee.EmployeeId == manager.ManagerId && employee.ManagerId == manager.EmployeeId)));
        }

        public bool AllManagersAreEmployees()
        {
            List<string> employeeIds = staffList.Select(s => s.EmployeeId).ToList();
            List<string> managerIds = staffList.Select(s => s.ManagerId).Where(id => !string.IsNullOrEmpty(id)).ToList();
            return !managerIds.Except(employeeIds).Any();
        }

        public long SalaryBudget(string employeeId)
        {
            EmployeeDto employee = staffList.FirstOrDefault(s => s.EmployeeId == employeeId);
            if (employee == null)
            {
                return -1;
            }

            return budgetUnder(employee);
        }


        private long budgetUnder(EmployeeDto employee)
        {
            long total = 0;
            long.TryParse(employee.Salary, out total);

            List<EmployeeDto> employees = staffList.Where(s => s.ManagerId == employee.EmployeeId).ToList();
            foreach (EmployeeDto newEmployee in employees)
            {
                total+= budgetUnder(newEmployee);
            }

            return total;
        }
    }


    public class EmployeeDto
    {
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public string Salary { get; set; }
    }
}