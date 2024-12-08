using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using Dapper;

namespace Globalization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MasterController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private async Task<IActionResult> ExecuteQueryAsync(string queryKey, string successMessage, string failureMessage)
        {
            try
            {
                var sqlQuery = _configuration[$"Queries:{queryKey}"];
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Query not found in configuration."
                    });
                }

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var data = await connection.QueryAsync<dynamic>(sqlQuery);

                    return Ok(new
                    {
                        Success = true,
                        Message = successMessage,
                        Data = data
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = failureMessage,
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("GetLanguages")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLanguages()
        {
            string successMessage = "Languages retrieved successfully.";
            string failureMessage = "Failed to retrieve Languages.";
            return await ExecuteQueryAsync("GetLanguages", successMessage, failureMessage);
        }

        [HttpPost]
        [Route("GetLicensePlateCountries")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLicensePlateCountries()
        {
            string successMessage = "License Plate Countries retrieved successfully.";
            string failureMessage = "Failed to retrieve License Plate Countries.";
            return await ExecuteQueryAsync("GetLicensePlateCountries", successMessage, failureMessage);
        }

        [HttpPost]
        [Route("GetLicensePlateStates")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLicensePlateStates()
        {
            string successMessage = "License Plate States retrieved successfully.";
            string failureMessage = "Failed to retrieve License Plate States.";
            return await ExecuteQueryAsync("GetLicensePlateStates", successMessage, failureMessage);
        }

        [HttpPost]
        [Route("GetAccountTypes")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccountTypes()
        {
            string successMessage = "Account Types retrieved successfully.";
            string failureMessage = "Failed to retrieve Account Types.";
            return await ExecuteQueryAsync("GetAccountTypes", successMessage, failureMessage);
        }

        [HttpPost]
        [Route("GetAccountStatuses")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccountStatuses()
        {
            string successMessage = "Account Statuses retrieved successfully.";
            string failureMessage = "Failed to retrieve Account Statuses.";
            return await ExecuteQueryAsync("GetAccountStatuses", successMessage, failureMessage);
        }

        [HttpPost]
        [Route("GetAttributesOrFlags")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAttributesOrFlags()
        {
            string successMessage = "Attributes/Flags retrieved successfully.";
            string failureMessage = "Failed to retrieve Attributes/Flags.";
            return await ExecuteQueryAsync("GetAttributesOrFlags", successMessage, failureMessage);
        }
    }
}
