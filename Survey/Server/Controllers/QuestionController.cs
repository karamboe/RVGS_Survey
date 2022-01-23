using Survey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Survey.Server.Services;

namespace Survey.Server.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> logger;
        private IQuestionDataService questionDataService { get; set; }

        public QuestionController(ILogger<QuestionController> logger, IQuestionDataService questionDataService)
        {
            this.logger = logger;
            this.questionDataService = questionDataService;
        }

        [HttpGet("ListQuestions/{id}")]
        public async Task<ActionResult> ListQuestions(string id)
        {
            try
            {
                var questions = await questionDataService.ListQuestions(id);

                if (questions == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No data found in the database");
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.ListQuestions(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetQuestionById/{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(string id)
        {
            try
            {
                var result = await questionDataService.GetQuestion(id);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.GetQuestionById(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("SaveQuestion/{item}")]
        public async Task<ActionResult<QuestionDto>> SaveQuestion(QuestionDto item)
        {
            try
            {
                var res = await questionDataService.SaveQuestion(item);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.SaveQuestion(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<ActionResult<bool>> DeleteQuestion(string id)
        {
            try
            {
                var res = await questionDataService.DeleteQuestion(id);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.DeleteQuestion(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListQuestionAlternatives/{id}")]
        public async Task<ActionResult> ListQuestionAlternatives(string id)
        {
            try
            {
                var questions = await questionDataService.ListAlternatives(id);

                if (questions == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No data found in the database");
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.ListQuestionAlternatives(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("SaveQuestionAlternative/{item}")]
        public async Task<ActionResult<bool>> SaveQuestionAlternative(AlternativeDto item)
        {
            try
            {
                var res = await questionDataService.SaveAlternative(item);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.SaveQuestionAlternative(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteQuestionAlternative/{id}")]
        public async Task<ActionResult<bool>> DeleteQuestionAlternative(string id)
        {
            try
            {
                var res = await questionDataService.DeleteAlternative(id);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in QuestionController.DeleteQuestionAlternative(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
