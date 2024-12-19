using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IAPIService _apiService;
        private readonly IAPIDocumentationService _documentationService;
        private readonly IAPIVersionService _versionService;
        private readonly IFeaturedAPIService _featuredAPIService;

        public APIController(IAPIService apiService, 
            IAPIDocumentationService documentationService, 
            IAPIVersionService versionService, 
            IFeaturedAPIService featuredAPIService)
        {
            _apiService = apiService;
            _documentationService = documentationService;
            _versionService = versionService;
            _featuredAPIService = featuredAPIService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<API>>>> GetAllAPIs()
        {
            try
            {
                var apis = await _apiService.GetAllAPIsAsync();
                return Ok(new Response<IEnumerable<API>>
                {
                    Success = true,
                    Message = "APIs retrieved successfully",
                    Data = apis
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<API>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving APIs: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<API>>> GetAPIById(int id)
        {
            try
            {
                var api = await _apiService.GetAPIByIdAsync(id);
                if (api == null)
                {
                    return NotFound(new Response<API>
                    {
                        Success = false,
                        Message = $"API with ID {id} not found"
                    });
                }

                return Ok(new Response<API>
                {
                    Success = true,
                    Message = "API retrieved successfully",
                    Data = api
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<API>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving API: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response<API>>> CreateAPI([FromBody] API api)
        {
            try
            {
                var createdApi = await _apiService.CreateAPIAsync(api);
                return CreatedAtAction(nameof(GetAPIById),
                    new { id = createdApi.Id },
                    new Response<API>
                    {
                        Success = true,
                        Message = "API created successfully",
                        Data = createdApi
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<API>
                {
                    Success = false,
                    Message = $"An error occurred while creating API: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<API>>> UpdateAPI(int id, [FromBody] API updatedApi)
        {
            try
            {
                var api = await _apiService.UpdateAPIAsync(id, updatedApi);
                if (api == null)
                {
                    return NotFound(new Response<API>
                    {
                        Success = false,
                        Message = $"API with ID {id} not found"
                    });
                }

                return Ok(new Response<API>
                {
                    Success = true,
                    Message = "API updated successfully",
                    Data = api
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<API>
                {
                    Success = false,
                    Message = $"An error occurred while updating API: {ex.Message}"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteAPI(int id)
        {
            try
            {
                var success = await _apiService.DeleteAPIAsync(id);
                if (!success)
                {
                    return NotFound(new Response<bool>
                    {
                        Success = false,
                        Message = $"API with ID {id} not found"
                    });
                }

                return Ok(new Response<bool>
                {
                    Success = true,
                    Message = "API deleted successfully",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting API: {ex.Message}"
                });
            }
        }

        // Documentation endpoints
        [HttpGet("documentation")]
        public async Task<ActionResult<Response<IEnumerable<APIDocumentation>>>> GetAllDocumentations()
        {
            try
            {
                var docs = await _documentationService.GetAllDocumentationsAsync();
                return Ok(new Response<IEnumerable<APIDocumentation>>
                {
                    Success = true,
                    Message = "Documentation retrieved successfully",
                    Data = docs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<APIDocumentation>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving documentation: {ex.Message}"
                });
            }
        }

        [HttpGet("documentation/{id}")]
        public async Task<ActionResult<Response<APIDocumentation>>> GetDocumentationById(int id)
        {
            try
            {
                var documentation = await _documentationService.GetDocumentationByIdAsync(id);
                if (documentation == null)
                {
                    return NotFound(new Response<APIDocumentation>
                    {
                        Success = false,
                        Message = $"Documentation with ID {id} not found"
                    });
                }

                return Ok(new Response<APIDocumentation>
                {
                    Success = true,
                    Message = "Documentation retrieved successfully",
                    Data = documentation
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIDocumentation>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving documentation: {ex.Message}"
                });
            }
        }

        [HttpPost("documentation")]
        public async Task<ActionResult<Response<APIDocumentation>>> CreateDocumentation([FromBody] APIDocumentation documentation)
        {
            try
            {
                var createdDoc = await _documentationService.CreateDocumentationAsync(documentation);
                return CreatedAtAction(nameof(GetDocumentationById),
                    new { id = createdDoc.Id },
                    new Response<APIDocumentation>
                    {
                        Success = true,
                        Message = "Documentation created successfully",
                        Data = createdDoc
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIDocumentation>
                {
                    Success = false,
                    Message = $"An error occurred while creating documentation: {ex.Message}"
                });
            }
        }

        [HttpPut("documentation/{id}")]
        public async Task<ActionResult<Response<APIDocumentation>>> UpdateDocumentation(int id, [FromBody] APIDocumentation updatedDoc)
        {
            try
            {
                var documentation = await _documentationService.UpdateDocumentationAsync(id, updatedDoc);
                if (documentation == null)
                {
                    return NotFound(new Response<APIDocumentation>
                    {
                        Success = false,
                        Message = $"Documentation with ID {id} not found"
                    });
                }

                return Ok(new Response<APIDocumentation>
                {
                    Success = true,
                    Message = "Documentation updated successfully",
                    Data = documentation
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIDocumentation>
                {
                    Success = false,
                    Message = $"An error occurred while updating documentation: {ex.Message}"
                });
            }
        }

        [HttpDelete("documentation/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteDocumentation(int id)
        {
            try
            {
                var success = await _documentationService.DeleteDocumentationAsync(id);
                if (!success)
                {
                    return NotFound(new Response<bool>
                    {
                        Success = false,
                        Message = $"Documentation with ID {id} not found"
                    });
                }

                return Ok(new Response<bool>
                {
                    Success = true,
                    Message = "Documentation deleted successfully",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting documentation: {ex.Message}"
                });
            }
        }

        // Version endpoints
        [HttpGet("version")]
        public async Task<ActionResult<Response<IEnumerable<APIVersion>>>> GetAllVersions()
        {
            try
            {
                var versions = await _versionService.GetAllVersionsAsync();
                return Ok(new Response<IEnumerable<APIVersion>>
                {
                    Success = true,
                    Message = "Versions retrieved successfully",
                    Data = versions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<APIVersion>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving versions: {ex.Message}"
                });
            }
        }

        [HttpGet("version/{id}")]
        public async Task<ActionResult<Response<APIVersion>>> GetVersionById(int id)
        {
            try
            {
                var version = await _versionService.GetVersionByIdAsync(id);
                if (version == null)
                {
                    return NotFound(new Response<APIVersion>
                    {
                        Success = false,
                        Message = $"Version with ID {id} not found"
                    });
                }

                return Ok(new Response<APIVersion>
                {
                    Success = true,
                    Message = "Version retrieved successfully",
                    Data = version
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIVersion>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving version: {ex.Message}"
                });
            }
        }

        [HttpPost("version")]
        public async Task<ActionResult<Response<APIVersion>>> CreateVersion([FromBody] APIVersion version)
        {
            try
            {
                var createdVersion = await _versionService.CreateVersionAsync(version);
                return CreatedAtAction(nameof(GetVersionById),
                    new { id = createdVersion.Id },
                    new Response<APIVersion>
                    {
                        Success = true,
                        Message = "Version created successfully",
                        Data = createdVersion
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIVersion>
                {
                    Success = false,
                    Message = $"An error occurred while creating version: {ex.Message}"
                });
            }
        }

        [HttpPut("version/{id}")]
        public async Task<ActionResult<Response<APIVersion>>> UpdateVersion(int id, [FromBody] APIVersion updatedVersion)
        {
            try
            {
                var version = await _versionService.UpdateVersionAsync(id, updatedVersion);
                if (version == null)
                {
                    return NotFound(new Response<APIVersion>
                    {
                        Success = false,
                        Message = $"Version with ID {id} not found"
                    });
                }

                return Ok(new Response<APIVersion>
                {
                    Success = true,
                    Message = "Version updated successfully",
                    Data = version
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<APIVersion>
                {
                    Success = false,
                    Message = $"An error occurred while updating version: {ex.Message}"
                });
            }
        }

        [HttpDelete("version/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteVersion(int id)
        {
            try
            {
                var success = await _versionService.DeleteVersionAsync(id);
                if (!success)
                {
                    return NotFound(new Response<bool>
                    {
                        Success = false,
                        Message = $"Version with ID {id} not found"
                    });
                }

                return Ok(new Response<bool>
                {
                    Success = true,
                    Message = "Version deleted successfully",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting version: {ex.Message}"
                });
            }
        }
        
        // Featured endpoints
        [HttpGet("featured")]
        public async Task<ActionResult<Response<IEnumerable<FeaturedAPI>>>> GetAllFeaturedAPIs()
        {
            try
            {
                var featuredAPIs = await _featuredAPIService.GetAllFeaturedAPIsAsync();
                return Ok(new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = true,
                    Message = "Featured APIs retrieved successfully",
                    Data = featuredAPIs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving featured APIs: {ex.Message}"
                });
            }
        }

        [HttpGet("featured/current")]
        public async Task<ActionResult<Response<IEnumerable<FeaturedAPI>>>> GetCurrentFeaturedAPIs()
        {
            try
            {
                var featuredAPIs = await _featuredAPIService.GetCurrentFeaturedAPIsAsync();
                return Ok(new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = true,
                    Message = "Current featured APIs retrieved successfully",
                    Data = featuredAPIs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving current featured APIs: {ex.Message}"
                });
            }
        }

        [HttpGet("featured/{id}")]
        public async Task<ActionResult<Response<FeaturedAPI>>> GetFeaturedAPIById(int id)
        {
            try
            {
                var featuredAPI = await _featuredAPIService.GetFeaturedAPIByIdAsync(id);
                if (featuredAPI == null)
                {
                    return NotFound(new Response<FeaturedAPI>
                    {
                        Success = false,
                        Message = $"Featured API with ID {id} not found"
                    });
                }

                return Ok(new Response<FeaturedAPI>
                {
                    Success = true,
                    Message = "Featured API retrieved successfully",
                    Data = featuredAPI
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<FeaturedAPI>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving featured API: {ex.Message}"
                });
            }
        }

        [HttpGet("featured/user/{userId}")]
        public async Task<ActionResult<Response<IEnumerable<FeaturedAPI>>>> GetFeaturedAPIsByUserId(int userId)
        {
            try
            {
                var featuredAPIs = await _featuredAPIService.GetFeaturedAPIsByUserIdAsync(userId);
                return Ok(new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = true,
                    Message = "Featured APIs for user retrieved successfully",
                    Data = featuredAPIs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<FeaturedAPI>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving featured APIs for user: {ex.Message}"
                });
            }
        }

        [HttpPost("featured")]
        public async Task<ActionResult<Response<FeaturedAPI>>> CreateFeaturedAPI([FromBody] FeaturedAPI featuredAPI)
        {
            try
            {
                var createdFeaturedAPI = await _featuredAPIService.CreateFeaturedAPIAsync(featuredAPI);
                return CreatedAtAction(nameof(GetFeaturedAPIById),
                    new { id = createdFeaturedAPI.Id },
                    new Response<FeaturedAPI>
                    {
                        Success = true,
                        Message = "Featured API created successfully",
                        Data = createdFeaturedAPI
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<FeaturedAPI>
                {
                    Success = false,
                    Message = $"An error occurred while creating featured API: {ex.Message}"
                });
            }
        }

        [HttpPut("featured/{id}")]
        public async Task<ActionResult<Response<FeaturedAPI>>> UpdateFeaturedAPI(int id, [FromBody] FeaturedAPI featuredAPI)
        {
            try
            {
                var updatedFeaturedAPI = await _featuredAPIService.UpdateFeaturedAPIAsync(id, featuredAPI);
                if (updatedFeaturedAPI == null)
                {
                    return NotFound(new Response<FeaturedAPI>
                    {
                        Success = false,
                        Message = $"Featured API with ID {id} not found"
                    });
                }

                return Ok(new Response<FeaturedAPI>
                {
                    Success = true,
                    Message = "Featured API updated successfully",
                    Data = updatedFeaturedAPI
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<FeaturedAPI>
                {
                    Success = false,
                    Message = $"An error occurred while updating featured API: {ex.Message}"
                });
            }
        }

        [HttpDelete("featured/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteFeaturedAPI(int id)
        {
            try
            {
                var success = await _featuredAPIService.DeleteFeaturedAPIAsync(id);
                if (!success)
                {
                    return NotFound(new Response<bool>
                    {
                        Success = false,
                        Message = $"Featured API with ID {id} not found"
                    });
                }

                return Ok(new Response<bool>
                {
                    Success = true,
                    Message = "Featured API deleted successfully",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting featured API: {ex.Message}"
                });
            }
        }
    }
}