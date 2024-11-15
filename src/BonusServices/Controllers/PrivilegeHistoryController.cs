using Microsoft.AspNetCore.Mvc;
using BonusServices.Services;
using ModelDTO.Bonus;

namespace BonusServices.Controllers
{
    [ApiController]
    [Route("api/v1/privilegeHistory")]
    public class PrivilegeHistoryController : ControllerBase
    {
        private readonly PrivilegeHistoryService privilegeHistoryServices;

        public PrivilegeHistoryController(PrivilegeHistoryService privilegeHistoryServices)
        {
            this.privilegeHistoryServices = privilegeHistoryServices;
        }

        private PrivilegeHistoryDTO changeDTO(PrivilegeHistory privilegeHistory)
        {
            return new PrivilegeHistoryDTO()
            {
                Id = privilegeHistory.id,
                PrivilegeId = privilegeHistory.privilege_id,
                TicketUid = privilegeHistory.ticket_uid,
                Datetime = privilegeHistory.datetime,
                BalanceDiff = privilegeHistory.balance_diff,
                OperationType = privilegeHistory.operation_type
            };
        }

        private List<PrivilegeHistoryDTO> listPivilegeHistoryDTO(List<PrivilegeHistory> privilegeHistorys)
        {
            List<PrivilegeHistoryDTO> privilegeHistoryDTOs = new List<PrivilegeHistoryDTO>();
            foreach (var privilegeHistory in privilegeHistorys)
            {
                var privilegeHistoryDTO = changeDTO(privilegeHistory);
                privilegeHistoryDTOs.Add(privilegeHistoryDTO);
            }
            return privilegeHistoryDTOs;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PrivilegeHistoryDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPivilegeHistory([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            List<PrivilegeHistory> privilegeHistorys = await privilegeHistoryServices.GetAll(page, size);
            List<PrivilegeHistoryDTO> lPivilegeHistoryDTO = listPivilegeHistoryDTO(privilegeHistorys);
            return Ok(lPivilegeHistoryDTO);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrivilegeHistoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByID(int id)
        {
            var privilegeHistory = await privilegeHistoryServices.GetPrivilegeHistoryById(id);
            var response = changeDTO(privilegeHistory);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(PrivilegeHistoryDTO))]
        public async Task<IActionResult> DeleteprivilegeHistory(int id)
        {
            await privilegeHistoryServices.Delete(id);
            return NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPrivilegeHistory([FromBody] PrivilegeHistoryDTO privilegeHistoryDTO)
        {
            var privilegeHistory = privilegeHistoryDTO.changeToDB(privilegeHistoryDTO);
            var result = await privilegeHistoryServices.Create(privilegeHistory);

            if (result is null)
            {
                return BadRequest();
            }

            var header = $"api/v1/persons/{result.id}";
            return Created(header, privilegeHistoryDTO);
        }
    }
}
