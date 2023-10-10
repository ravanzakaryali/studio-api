using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Space.Application.DTOs.Program.Request;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Program controller
/// </summary>
public class ProgramsController : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all programs.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a list of all programs upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with roles "admin," "mentor," "ta," or "muellim" to retrieve a list of all programs.
    /// It returns a list of programs as an HTTP response upon successful retrieval.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllProgramsQuery()));
    }

    /// <summary>
    /// Retrieves details of a specific program based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the program to retrieve details for.</param>
    /// <returns>
    /// An HTTP response containing details of the specified program upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with roles "admin," "mentor," "ta," or "muellim" to retrieve details
    /// of a specific program based on its unique identifier. It expects the unique identifier of the program as a route
    /// parameter and returns the program's details as an HTTP response upon successful retrieval.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetProgramQuery(id)));

    /// <summary>
    /// Creates a new program with the provided details.
    /// </summary>
    /// <param name="program">A JSON object containing details for creating the program.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful creation of the program.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to create a new program by providing a JSON object
    /// with the necessary details, including the program name and total hours. It returns a 204 status code upon
    /// successful creation of the program.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateProgramRequestDto program)
    {
        await Mediator.Send(new CreateProgramCommand(program.Name, program.TotalHours));
        return NoContent();
    }

    /// <summary>
    /// Deletes a program based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the program to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful deletion of the program.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to delete a program based on its unique identifier.
    /// It expects the unique identifier of the program as a route parameter. Upon successful deletion, it returns
    /// an HTTP response with a status code 204 (No Content) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteProgramCommand(id));
        return NoContent();
    }
}
