using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Implementations;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet("get-faqs")]
        public async Task<IActionResult> GetAllFAQs()
        {
            try
            {
                var faqs = await _faqService.GetAllFAQsAsync();
                return Ok(new Response<IEnumerable<FAQ>>
                {
                    Success = true,
                    Message = "FAQs retrieved successfully.",
                    Data = faqs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while fetching FAQs.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("get-faq-by-id")]
        public async Task<IActionResult> GetFAQById(int id)
        {
            try
            {
                var faq = await _faqService.GetFAQByIdAsync(id);
                if (faq == null)
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = $"FAQ with ID {id} not found."
                    });
                }
                return Ok(new Response<FAQ>
                {
                    Success = true,
                    Message = "FAQ retrieved successfully.",
                    Data = faq
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while fetching the FAQ.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("get-faq-by-userId")]
        public async Task<IActionResult> GetFAQsByUserId(int userId)
        {
            try
            {
                var faqs = await _faqService.GetFAQsByUserIdAsync(userId);
                if (!faqs.Any())
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = $"No FAQs found for User ID {userId}."
                    });
                }
                return Ok(new Response<IEnumerable<FAQ>>
                {
                    Success = true,
                    Message = "FAQs retrieved successfully.",
                    Data = faqs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while fetching FAQs by user.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("create-faq")]
        public async Task<IActionResult> CreateFAQ([FromBody] FAQ faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Invalid data provided.",
                    Data = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var createdFAQ = await _faqService.CreateFAQAsync(faq);
                return CreatedAtAction(nameof(GetFAQById), new { id = createdFAQ.Id }, new Response<FAQ>
                {
                    Success = true,
                    Message = "FAQ created successfully.",
                    Data = createdFAQ
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while creating the FAQ.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("update-faq")]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] FAQ faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Invalid data provided.",
                    Data = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var updatedFAQ = await _faqService.UpdateFAQAsync(id, faq);
                if (updatedFAQ == null)
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = $"FAQ with ID {id} not found."
                    });
                }
                return Ok(new Response<FAQ>
                {
                    Success = true,
                    Message = "FAQ updated successfully.",
                    Data = updatedFAQ
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the FAQ.",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("delete-faq")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                var isDeleted = await _faqService.DeleteFAQAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = $"FAQ with ID {id} not found or already deleted."
                    });
                }
                return Ok(new Response<string>
                {
                    Success = true,
                    Message = "FAQ deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while deleting the FAQ.",
                    Data = ex.Message
                });
            }
        }
    }
}
