using Microsoft.AspNetCore.Mvc;
using ModelDTO.Bonus;
using BonusServices.Services;

namespace TicketServices.Controllers
{
    [ApiController]
    [Route("api/v1/privilege")]
    public class PrivilegeController : ControllerBase
    {
        private readonly PrivilegeService privilegeServices;

        public PrivilegeController(PrivilegeService privilegeServices)
        {
            this.privilegeServices = privilegeServices;
        }

        private PrivilegeDTO changeDTO(Privilege privilege)
        {
            return new PrivilegeDTO()
            {
                Id = privilege.id,
                Username = privilege.username,
                Status = privilege.status,
                Balance = privilege.balance
            };
        }

        private List<PrivilegeDTO> listPrivilegeDTO(List<Privilege> privileges)
        {
            List<PrivilegeDTO> privilegeDTOs = new List<PrivilegeDTO>();
            foreach (var privilege in privileges)
            {
                var privilegeDTO = changeDTO(privilege);
                privilegeDTOs.Add(privilegeDTO);
            }
            return privilegeDTOs;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PrivilegeDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPrivilege([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            List<Privilege> privileges = await privilegeServices.GetAll(page, size);
            List<PrivilegeDTO> lprivilegeDTO = listPrivilegeDTO(privileges);
            return Ok(lprivilegeDTO);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrivilegeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByID(int id)
        {
            var privilege = await privilegeServices.GetPrivilegeById(id);
            var response = changeDTO(privilege);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrivilegeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByUsername([FromRoute] string username)
        {
            var privilege = await privilegeServices.GetPrivilegeByUsername(username);
            var response = changeDTO(privilege);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(PrivilegeDTO))]
        public async Task<IActionResult> DeletePrivilege(int id)
        {
            await privilegeServices.Delete(id);
            return NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPrivilege([FromBody] PrivilegeDTO privilegeDTO)
        {
            var privilege = privilegeDTO.changeToDB(privilegeDTO);
            var result = await privilegeServices.Create(privilege);

            if (result is null)
            {
                return BadRequest();
            }

            var header = $"api/v1/persons/{result.id}";
            return Created(header, privilege);
        }
        [HttpPost("/back_bonus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Back_bonus([FromBody] AddReq privilegeDTO)
        {
            await privilegeServices.BackBonuses(privilegeDTO);
            return Ok();
        }
        void FixedPatchFields(Privilege privilegeToPatch, Privilege userPrivilege)
        {
            if (userPrivilege.username != null && userPrivilege.username != "string")
                privilegeToPatch.username = userPrivilege.username;
            if (userPrivilege.status != null && userPrivilege.status != "string")
                privilegeToPatch.status = userPrivilege.status;
            privilegeToPatch.balance = userPrivilege.balance ;
        }
        [HttpPatch]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrivilegeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrivilege([FromRoute(Name = "Id")] int id, [FromBody] PrivilegeDTO privilegeDTO)
        {
            var privilegeToPatch = await privilegeServices.GetPrivilegeById(id);
            if (privilegeToPatch is null)
                return NotFound();
            var userPerson = privilegeDTO.changeToDB(privilegeDTO);
            FixedPatchFields(privilegeToPatch, userPerson);
            await privilegeServices.Update(privilegeToPatch);
            var updatedPrivilege = changeDTO(privilegeToPatch);
            return Ok(updatedPrivilege);
        }
    }
}
