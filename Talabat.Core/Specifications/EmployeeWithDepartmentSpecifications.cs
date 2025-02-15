

namespace Talabat.Core.Specifications
{
    public class EmployeeWithDepartmentSpecifications:BaseSpecifications<Employee>
    {
        public EmployeeWithDepartmentSpecifications()
        {

            Includes.Add(p => p.Department);
        }

        // ctor is used for get product by id
        public EmployeeWithDepartmentSpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Department);
        }
    }
}
