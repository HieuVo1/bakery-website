using System.Threading.Tasks;
using eShopSolution.Application.Languages;
using eShopSolution.ViewModel.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var languages = await _languageService.GetAll();
            return Ok(languages);
        }
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetById(string languageId)
        {
            var result = await _languageService.GetById(languageId);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanguageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _languageService.Create(request);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{languageId}")]
        public async Task<IActionResult> Update([FromBody] LanguageUpdateRequest request,string languageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _languageService.Update(request, languageId);
            if (result.IsSuccessed==false) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{languageId}")]

        public async Task<IActionResult> Delete(string languageId)
        {
            var result = await _languageService.Delete(languageId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }

    }
}