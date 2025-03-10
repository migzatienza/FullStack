using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using FullStack.API.Models;
using SQLitePCL;

namespace FullStack.API.DAO
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FullStackConnectionString");
        }
        
        public async Task<List<Employee>> GetAllEmployee()
        {
            var employee = new List<Employee>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("FetchAllEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employee.Add(new Employee
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetInt64(reader.GetOrdinal("Phone")),
                                Salary = reader.GetInt64(reader.GetOrdinal("Salary")),
                                Department = reader.GetString(reader.GetOrdinal("Department"))
                            });
                        }
                    }
                }
            }
            return employee;
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            Employee employee = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("FetchEmployeesById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            employee = new Employee
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetInt64(reader.GetOrdinal("Phone")),
                                Salary = reader.GetInt64(reader.GetOrdinal("Salary")),
                                Department = reader.GetString(reader.GetOrdinal("Department"))
                            };
                        }
                    }
                }
            }
            return employee;
        }

        public async Task<Task> AddEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar) { Value = employee.Name });
                    command.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar) { Value = employee.Email });
                    command.Parameters.Add(new SqlParameter("@p3", SqlDbType.BigInt) { Value = employee.Phone });
                    command.Parameters.Add(new SqlParameter("@p4", SqlDbType.BigInt) { Value = employee.Salary });
                    command.Parameters.Add(new SqlParameter("@p5", SqlDbType.NVarChar) { Value = employee.Department });
                    command.ExecuteNonQuery();
                }
            }
            return Task.CompletedTask;
        }
        public async Task UpdateEmployee(Guid id, Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using(var command = new SqlCommand("UpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", employee.Id);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    await command.ExecuteNonQueryAsync();
                }
            }
            
        }

        public async Task<Task> DeleteEmployee(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("DeleteEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });
                    command.ExecuteNonQuery();
                }
            }
            return Task.CompletedTask;
        }

        internal void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
