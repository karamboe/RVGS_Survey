using Survey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Survey.Server.Services;

namespace Survey.Server.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly ILogger<SurveyController> logger;
        private ISurveyDataService surveyDataService { get; set; }

        public SurveyController(ILogger<SurveyController> logger, ISurveyDataService surveyDataService)
        {
            this.logger = logger;
            this.surveyDataService = surveyDataService;
        }

        [HttpGet, ActionName("List")]
        public async Task<ActionResult> List()
        {
            try
            {
                var survs = await surveyDataService.ListSurveys();

                if (survs == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No data found in the database");
                }

                return Ok(survs);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in SurveyController.List(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}"), ActionName("GetById")]
        public async Task<ActionResult<SurveyDto>> GetById(string id)
        {
            try
            {
                var result = await surveyDataService.GetSurvey(id);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in SurveyController.GetById(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost()]
        public async Task<ActionResult<bool>> Save(SurveyDto item)
        {
            try
            {
                var res = await surveyDataService.SaveSurvey(item);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in SurveyController.Save(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                var res = await surveyDataService.DeleteSurvey(id);
                return Ok(res); // NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in SurveyController.Delete(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }


    }
}
