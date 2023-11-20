namespace Space.WebAPI.Controllers;

/// <summary>
/// Module controller
/// </summary>
[Authorize(Roles = "admin")]
public class ModulesController : BaseApiController
{
    /// <summary>
    /// Creates new modules for a program based on the provided details.
    /// </summary>
    /// <param name="modules">A JSON object containing details for creating the modules.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) and a list of created modules upon successful creation.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to create new modules for a program by providing a JSON object with the necessary details,
    /// including the program identifier and a list of modules. It returns a 201 status code along with a list of created modules upon
    /// successful creation.
    /// </remarks>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<GetModuleDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequestDto modules)
        => StatusCode(201, await Mediator.Send(new CreateModuleCommand(modules.ProgramId, modules.Modules)));

    /// <summary>
    /// Deletes a module based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the module to delete.</param>
    /// <returns>
    /// An HTTP response indicating the success of the module deletion.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to delete a module based on its unique identifier.
    /// It expects the unique identifier of the module as a route parameter. Upon successful deletion,
    /// it returns an HTTP response with a status code 204 (No Content) to indicate the success of the operation.
    /// If the module with the specified identifier is not found, it returns a 404 (Not Found) status code.
    /// </remarks>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteModuleCommand(id));
        return StatusCode(204);
    }

    /// <summary>
    /// Retrieves a list of all modules.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a list of all modules upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve a list of all modules. It returns a list of modules
    /// as an HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllModuleQuery()));
}
