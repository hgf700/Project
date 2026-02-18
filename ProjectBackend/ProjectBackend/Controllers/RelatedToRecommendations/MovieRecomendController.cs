using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectBackend.Controllers.RelatedToRecommendations;

[Authorize]
[ApiController]
[Route("recommendations")]
public class MovieRecomendController : ControllerBase
{
    public MovieRecomendController(
        
        )
    {

    }


}
