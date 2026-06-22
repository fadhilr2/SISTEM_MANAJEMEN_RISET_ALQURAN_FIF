using API.Dtos;
using Lib.services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/papers")]
    [Produces("application/json")]
    [Tags("Papers")]
    public class PaperController : ControllerBase
    {
        // ── GET /api/papers ──────────────────────────────────────────────────

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaperDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var papers = DataContext.Papers.GetAll()
                .Select(p => new PaperDto(p.Title, p.Paper_Abstract, p.Author, p.Date));

            return Ok(ApiResponse.Ok(papers));
        }

        // ── GET /api/papers/{title} ──────────────────────────────────────────

        [HttpGet("{title}")]
        [ProducesResponseType(typeof(ApiResponse<PaperDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult GetByTitle(string title)
        {
            var paper = PaperService.SearchPaper(title);
            if (paper == null)
                return NotFound(ApiResponse.Fail($"Paper '{title}' not found."));

            return Ok(ApiResponse.Ok(new PaperDto(paper.Title, paper.Paper_Abstract, paper.Author, paper.Date)));
        }

        // ── POST /api/papers ─────────────────────────────────────────────────

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        public IActionResult Upload([FromBody] UploadPaperRequest body)
        {
            if (Session.Instance.Account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            var role = Session.Instance.Account.Role;
            if (!role.Equals("researcher", StringComparison.OrdinalIgnoreCase)
             && !role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Only researchers or admins can upload papers."));

            bool ok = PaperService.UploadPaper(body.Title, body.Abstract);
            if (!ok)
                return Unauthorized(ApiResponse.Fail("Upload failed — no active session."));

            return StatusCode(StatusCodes.Status201Created,
                ApiResponse.Ok($"Paper '{body.Title}' uploaded successfully."));
        }

        // ── PATCH /api/papers/title ──────────────────────────────────────────

        [HttpPatch("title")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult EditTitle([FromBody] EditTitleRequest body)
        {
            if (Session.Instance.Account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            var paper = PaperService.SearchPaper(body.CurrentTitle);
            if (paper == null)
                return NotFound(ApiResponse.Fail($"Paper '{body.CurrentTitle}' not found."));

            bool ok = PaperService.EditTitle(paper, body.NewTitle);
            if (!ok)
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("You do not have permission to edit this paper."));

            return Ok(ApiResponse.Ok("Title updated successfully."));
        }

        // ── PATCH /api/papers/abstract ───────────────────────────────────────

        [HttpPatch("abstract")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult EditAbstract([FromBody] EditAbstractRequest body)
        {
            if (Session.Instance.Account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            var paper = PaperService.SearchPaper(body.Title);
            if (paper == null)
                return NotFound(ApiResponse.Fail($"Paper '{body.Title}' not found."));

            bool ok = PaperService.EditAbstract(paper, body.NewAbstract);
            if (!ok)
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("You do not have permission to edit this paper."));

            return Ok(ApiResponse.Ok("Abstract updated successfully."));
        }

        // ── DELETE /api/papers ───────────────────────────────────────────────


        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromBody] DeletePaperRequest body)
        {
            if (Session.Instance.Account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            var paper = PaperService.SearchPaper(body.Title);
            if (paper == null)
                return NotFound(ApiResponse.Fail($"Paper '{body.Title}' not found."));

            bool ok = PaperService.DeletePaper(paper);
            if (!ok)
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("You do not have permission to delete this paper."));

            return Ok(ApiResponse.Ok($"Paper '{body.Title}' deleted."));
        }
    }
}
