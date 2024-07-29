using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.DTO.EmployeeDto;
using TaskManagementSystem.Models.DTO.LoginDto;
using TaskManagementSystem.Services.EmployeeService;
using TaskManagementSystem.Services.TokenService;

namespace TaskManagementSystem.Controllers
{
    //Controller for authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IEmployeeService employeeService;

        public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService, IEmployeeService employeeService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.employeeService = employeeService;
        }

        //Endpoint for registering a user, user can be assigned to three types of roles -> admin, manager and employee
        [HttpPost]
        [Route("Register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] AddEmployeeRequestDto addEmployeeRequestDto)
        {
            var user = await userManager.FindByEmailAsync(addEmployeeRequestDto.EmployeeEmail);

            if (user != null)
                return BadRequest("This employee email already exists");


            var identityUser = new IdentityUser
            {
                UserName = addEmployeeRequestDto.EmployeeEmail,
                Email = addEmployeeRequestDto.EmployeeEmail
            };

            
            var identityResult = await userManager.CreateAsync(identityUser, addEmployeeRequestDto.EmployeePassword);

            if (identityResult.Succeeded)
            {
                if (addEmployeeRequestDto.EmployeeRole != null)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, new List<string>() { addEmployeeRequestDto.EmployeeRole });

                    var response = await employeeService.AddEmployee(addEmployeeRequestDto);
                    if (identityResult.Succeeded && response != null)
                    {
                        return Ok("User is registered successfully! Please Login.");
                    }
                }
            }

            return BadRequest("Something went wrong");

        }

        //Endpoint for logging in a registered user
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (result)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwt = tokenService.CreateJWT(user, roles.ToList());
                        return Ok(jwt);
                    }
                }
            }

            return BadRequest("Username or password is incorrect");
        }
    }
}
