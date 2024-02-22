using Microsoft.AspNetCore.Mvc;
using WebService.API.Helpers;
using WebService.Core.Common;
using WebService.Core.Entities.Business;
using WebService.Core.Interfaces.IServices;

namespace WebService.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _DepartmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService DepartmentService)
        {
            _logger = logger;
            _DepartmentService = DepartmentService;
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
                            PropertyName = "Description",
                            Value = search,
                            Comparison = Comparison.Contains
                        }
                    });
                }

                var Departments = await _DepartmentService.GetPaginatedDataWithFilter(pageNumberValue, pageSizeValue, filters, cancellationToken);

                var response = new ResponseViewModel<PaginatedDataViewModel<DepartmentViewModel>>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = Departments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrievingDepartments");

                var errorResponse = new ResponseViewModel<IEnumerable<DepartmentViewModel>>
                {
                    Success = false,
                    Message = "Error retrievingDepartments",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpGet("get-all-with-relation")]
        public async Task<IActionResult> GetAllWithRelation(CancellationToken cancellationToken)
        {
            try
            {
                var Departments = await _DepartmentService.GetAllWithRelation(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<DepartmentViewModel>>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = Departments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrievingDepartments");

                var errorResponse = new ResponseViewModel<IEnumerable<DepartmentViewModel>>
                {
                    Success = false,
                    Message = "Error retrievingDepartments",
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
                var Departments = await _DepartmentService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<DepartmentViewModel>>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = Departments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrievingDepartments");

                var errorResponse = new ResponseViewModel<IEnumerable<DepartmentViewModel>>
                {
                    Success = false,
                    Message = "Error retrievingDepartments",
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
                var data = await _DepartmentService.GetAllWithByIdRelation(id, cancellationToken);

                var response = new ResponseViewModel<DepartmentViewModel>
                {
                    Success = true,
                    Message = "Department retrieved successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<DepartmentViewModel>
                    {
                        Success = false,
                        Message = "Department not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Department not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving theDepartment");

                var errorResponse = new ResponseViewModel<DepartmentViewModel>
                {
                    Success = false,
                    Message = "Error retrievingDepartment",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate(DepartmentCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _DepartmentService.IsExists("Name", model.Name, cancellationToken))
                {
                    message = $"TheDepartment name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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

                if (await _DepartmentService.IsExists("Description", model.Description, cancellationToken))
                {
                    message = $"The Department Description - '{model.Description}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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
                    var data = await _DepartmentService.BulkCreate(model, cancellationToken);

                    var response = new ResponseViewModel<DepartmentViewModel>
                    {
                        Success = true,
                        Message = "Department created successfully",
                        Data = data
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding theDepartment");
                    message = $"An error occurred while adding theDepartment- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<DepartmentViewModel>
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

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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


        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _DepartmentService.IsExists("Name", model.Name, cancellationToken))
                {
                    message = $"TheDepartment name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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

                if (await _DepartmentService.IsExists("Description", model.Description, cancellationToken))
                {
                    message = $"The Department Description - '{model.Description}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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
                    var data = await _DepartmentService.Create(model, cancellationToken);

                    var response = new ResponseViewModel<DepartmentViewModel>
                    {
                        Success = true,
                        Message = "Department created successfully",
                        Data = data
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding theDepartment");
                    message = $"An error occurred while adding theDepartment- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<DepartmentViewModel>
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

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<DepartmentViewModel>
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
        public async Task<IActionResult> Edit(DepartmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _DepartmentService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
                {
                    message = $"TheDepartment name- '{model.Name}' already exists";
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

                if (await _DepartmentService.IsExistsForUpdate(model.Id, "Description", model.Description, cancellationToken))
                {
                    message = $"The Department Description - '{model.Description}' already exists";
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
                    await _DepartmentService.Update(model, cancellationToken);

                    var response = new ResponseViewModel
                    {
                        Success = true,
                        Message = "Department updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating theDepartment");
                    message = $"An error occurred while updating theDepartment- " + ex.Message;

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
                await _DepartmentService.Delete(id, cancellationToken);

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Department deleted successfully"
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
                        Message = "Department not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Department not found"
                        }
                    });
                }

                _logger.LogError(ex, "An error occurred while deleting theDepartment");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting theDepartment",
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
