using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebService.API.Helpers;
using WebService.Core.Common;
using WebService.Core.Entities.Business;
using WebService.Core.Interfaces.IServices;

namespace WebService.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService EmployeeService)
        {
            _logger = logger;
            _EmployeeService = EmployeeService;
        }

        [HttpGet("paginated-data-with-filter")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, string? search, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;

                var filters = new List<ExpressionFilter>();
                if (!string.IsNullOrWhiteSpace(search) && search != null)
                {
                    // Add filters for relevant properties
                    filters.AddRange(new[]
                    {
                        new ExpressionFilter
                        {
                            PropertyName = "Name",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "EmployeeCode",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "Description",
                            Value = search,
                            Comparison = Comparison.Contains
                        }
                    });

                    // Check if the search string represents a valid numeric value for the "Price" property
                    if (double.TryParse(search, out double salary))
                    {
                        filters.Add(new ExpressionFilter
                        {
                            PropertyName = "Salary",
                            Value = salary,
                            Comparison = Comparison.Equal
                        });
                    }
                }

                var Employees = await _EmployeeService.GetPaginatedDataWithFilter(pageNumberValue, pageSizeValue, filters, cancellationToken);

                var response = new ResponseViewModel<PaginatedDataViewModel<EmployeeViewModel>>
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = Employees
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Employees");

                var errorResponse = new ResponseViewModel<IEnumerable<EmployeeViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving Employees",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var Employees = await _EmployeeService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<EmployeeViewModel>>
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = Employees
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Employees");

                var errorResponse = new ResponseViewModel<IEnumerable<EmployeeViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving Employees",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _EmployeeService.GetById(id, cancellationToken);

                var response = new ResponseViewModel<EmployeeViewModel>
                {
                    Success = true,
                    Message = "Employee retrieved successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<EmployeeViewModel>
                    {
                        Success = false,
                        Message = "Employee not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Employee not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the Employee");

                var errorResponse = new ResponseViewModel<EmployeeViewModel>
                {
                    Success = false,
                    Message = "Error retrieving Employee",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _EmployeeService.IsExists("Name", model.Name, cancellationToken))
                {
                    message = $"The Employee name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<EmployeeViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_NAME",
                            Message = message
                        }
                    });
                }

                if (await _EmployeeService.IsExists("EmployeeCode", model.EmployeeCode, cancellationToken))
                {
                    message = $"The Employee code- '{model.EmployeeCode}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<EmployeeViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_CODE",
                            Message = message
                        }
                    });
                }

                try
                {
                    var data = await _EmployeeService.Create(model, cancellationToken);

                    var response = new ResponseViewModel<EmployeeViewModel>
                    {
                        Success = true,
                        Message = "Employee created successfully",
                        Data = data
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the Employee");
                    message = $"An error occurred while adding the Employee- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<EmployeeViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<EmployeeViewModel>
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EmployeeUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _EmployeeService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
                {
                    message = $"The Employee name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_NAME",
                            Message = message
                        }
                    });
                }

                if (await _EmployeeService.IsExistsForUpdate(model.Id, "EmployeeCode", model.EmployeeCode, cancellationToken))
                {
                    message = $"The Employee code- '{model.EmployeeCode}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_CODE",
                            Message = message
                        }
                    });
                }

                try
                {
                    await _EmployeeService.Update(model, cancellationToken);

                    var response = new ResponseViewModel
                    {
                        Success = true,
                        Message = "Employee updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the Employee");
                    message = $"An error occurred while updating the Employee- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _EmployeeService.Delete(id, cancellationToken);

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Employee deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel
                    {
                        Success = false,
                        Message = "Employee not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Employee not found"
                        }
                    });
                }

                _logger.LogError(ex, "An error occurred while deleting the Employee");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting the Employee",
                    Error = new ErrorViewModel
                    {
                        Code = "DELETE_ROLE_ERROR",
                        Message = ex.Message
                    }
                });

            }
        }
    }

}
