using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private Kernel _kernel;
    public AIController(Kernel kernel)
    {
        _kernel = kernel;
    }

    [HttpPost]
    public async Task<IActionResult> AskAI([FromBody] string prompt)
    {
        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));
        return Ok(new { response = result.ToString() });
    }
}