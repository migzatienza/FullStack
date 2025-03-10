//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using FullStack.API.Data;
//using FullStack.API.Models;
//using System.Numerics;

//namespace FullStack.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeesController : ControllerBase
//    {
//        private readonly FullStackDbContext _context;

//        public EmployeesController(FullStackDbContext context) => _context = context;

//        // GET: api/Employees
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
//        {
//          if (_context.Employees == null)
//          {
//              return NotFound();
//          }
//            return await _context.Employees.FromSqlRaw("FetchAllEmployees").ToListAsync();
//        }

//        // GET: api/Employees/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
//        {
//          if (_context.Employees == null)
//          {
//              return NotFound();
//          }
//            var employee = await _context.Employees.FindAsync(id);

//            if (employee == null)
//            {
//                return NotFound();
//            }

//            return employee;
//        }

//        // PUT: api/Employees/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
//        {
//            if (id != employee.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(employee).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!EmployeeExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Employees
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<Employee>> PostEmployee(EmployeeCreateRequest request)
//        {
//            var res = await _context.Database.ExecuteSqlInterpolatedAsync
//                ($"EXEC AddEmployees @p1={request.Name}, @p2={request.Email}, @p3={request.Phone}, @p4={request.Salary}, @p5={request.Department}");
        
//            if(res == 0)
//            {
//              return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new user.");
//            }

//            return CreatedAtAction(nameof(PostEmployee), new { name = request.Name, email = request.Email }, null);
//        }

//        // DELETE: api/Employees/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEmployee(Guid id)
//        {
//            if (_context.Employees == null)
//            {
//                return NotFound();
//            }
//            var employee = await _context.Employees.FindAsync(id);
//            if (employee == null)
//            {
//                return NotFound();
//            }

//            _context.Employees.Remove(employee);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool EmployeeExists(Guid id)
//        {
//            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
