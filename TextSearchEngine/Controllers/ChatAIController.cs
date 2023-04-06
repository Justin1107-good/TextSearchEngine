using Microsoft.AspNetCore.Mvc; 
using OpenAiClient4ChatGPT;

namespace TextSearchEngine.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChatAIController : Controller
    {
        private readonly IOpenAiServices _openAi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openAi"></param>
        public ChatAIController(IOpenAiServices openAi)
        {
            _openAi = openAi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// AI机器人
        /// </summary>
        /// <returns></returns>
        [HttpPost] 
        public async Task<IActionResult> Chat(string question, string apiKey)
        {
            if (string.IsNullOrEmpty(question))
            {
                ViewData["question"] = "问题和你的秘钥ApiKey不能为空";
                return View("Error");
            }
            //这是你key
            if (string.IsNullOrEmpty(apiKey))
            {
                return View("Error");
            }
            var result = await _openAi.CallGPT3(question, apiKey);
             return Ok(result.choices);
        }
    }
}
