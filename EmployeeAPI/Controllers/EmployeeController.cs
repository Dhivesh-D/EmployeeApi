using EmployeeAPI.Data;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        //creating data context to access the DB...
        public EmployeeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //get method to fetch data from db master....
        [HttpGet]
        [Produces("application/xml")]
        public async Task<ActionResult<List<Employee>>> getEmployee()
        {
            var employee = await _dataContext.Employees.ToListAsync();

            Log.Information("Employee Details: {@employee}", employee);

            return Ok(employee);
        }

        //get method to fetch a specific employee. Accepts id as parameter
        [HttpGet("{id}")]
        [Produces("application/xml")]
        public async Task<ActionResult<List<Employee>>> getanEmployee(int id)
        {
            var employee = await _dataContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return BadRequest("Employee not found");
            }
            return Ok(employee);
        }
        
        //post method to add employees to the master db....
        //employing consumes attirubute to accept XML data
        [HttpPost]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public async Task<ActionResult<List<Employee>>> addEmployee(EmployeeWrapper wrapper)
        {
            //this is added to prevent from null exception being thrown. Removing this will result in a null pointer exception.
            if (wrapper.Employee == null)
            {
                return BadRequest("The Employee field is required.");
            }

            _dataContext.Employees.Add(wrapper.Employee);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Employees.ToListAsync());
        }

        //put method to update employee details
        //not the efficient way to return the whole list in a typical scenario.
        //have to update accordingly.... write a function to find ID and integrate with a UI.
        [HttpPut]
        public async Task<ActionResult<List<Employee>>> editEmployee(Employee editedEmployee)
        {
            var dbEmp = await _dataContext.Employees.FindAsync(editedEmployee.Id);
            if (dbEmp == null)
                return BadRequest("Employee not found");

            //edit the employee details
            dbEmp.FirstName = editedEmployee.FirstName;
            dbEmp.LastName = editedEmployee.LastName;
            dbEmp.Designation=editedEmployee.Designation;
            dbEmp.Email = editedEmployee.Email;

            await _dataContext.SaveChangesAsync();

            //returns the whole list since data is small. A confirmation msg might suffice

            return Ok(await _dataContext.Employees.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<Employee>>> deleteEmployee(int id)
        {
            var dbEmp = await _dataContext.Employees.FindAsync(id);
            if (dbEmp == null)
                return BadRequest("Employee not found");

            //remove the employee entry
            _dataContext.Employees.Remove(dbEmp);
            await _dataContext.SaveChangesAsync();

            //returns the whole list since data is small. A confirmation msg might suffice
            return Ok(await _dataContext.Employees.ToListAsync());
        }
    }
}
