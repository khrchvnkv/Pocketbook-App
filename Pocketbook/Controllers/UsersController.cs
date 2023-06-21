using Microsoft.AspNetCore.Mvc;
using Pocketbook.Core.IConfiguration;
using Pocketbook.Models;

namespace Pocketbook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUnitOfWork unitOfWork, ILogger<UsersController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll(); 
            return Ok(users); // 200
        }
        
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if (user is null) return NotFound(); //404

            return Ok(user); // 200
        }
        
        [HttpGet("get-fullname/{id}")]
        public async Task<IActionResult> GetUserFullName(Guid id)
        {
            var fullname = await _unitOfWork.Users.GetFirstNameAndLastName(id);
            if (fullname is null) return NotFound();

            return Ok(fullname);
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();

                await _unitOfWork.Users.Add(user);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetUser ), new { user.Id }, user);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 }; // 500 - server error
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(User user)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync(); 
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.Users.Delete(id);
            if (result)
            {
                await _unitOfWork.CompleteAsync(); 
                return Ok(id);
            }

            return BadRequest(id);
        }
    }
}