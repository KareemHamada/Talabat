namespace Talabat.Apis.Controllers
{

    public class EmployeeController : APIBaseController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeController(IGenericRepository<Employee> EmployeeRepo)
        {
            _employeeRepo = EmployeeRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployess()
        {
            var Spec = new EmployeeWithDepartmentSpecifications();

            var employees = await _employeeRepo.GetAllAsyncWithSpecifications(Spec);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var Spec = new EmployeeWithDepartmentSpecifications(id);

            var employee = await _employeeRepo.GetEntityAsyncWithSpecifications(Spec);
            return Ok(employee);
        }
    }
}
