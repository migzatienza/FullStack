using System.Net.Http.Headers;
using FullStack.API.DAO;
using FullStack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly EmployeeRepository _repository;

        public EmployeesAPIController(EmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _repository.GetAllEmployee();
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByID(Guid id)
        {
            var employees = await _repository.GetEmployeeById(id);
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            await _repository.AddEmployee(employee);
            return CreatedAtAction("GetEmployees", new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("Employee ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.UpdateEmployee(id, employee);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound($"Employee with ID {id} not found.");
                }
                else
                {
                    // Log the exception (assuming a logger is available)
                    // _logger.LogError(ex, "Concurrency error while updating employee with ID {EmployeeId}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError, "A concurrency error occurred while updating the employee.");
                }
            }
            catch (Exception)
            {
                // Log the exception (assuming a logger is available)
                // _logger.LogError(ex, "An error occurred while updating employee with ID {EmployeeId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the employee.");
            }

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            await _repository.DeleteEmployee(id);
            return NoContent();
        }
    }
}
