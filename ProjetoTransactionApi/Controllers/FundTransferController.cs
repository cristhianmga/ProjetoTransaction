using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTransactionApplication.Dtos;
using ProjetoTransactionApplication.Interfaces;
using ProjetoTransactionQueue.Interfaces;

namespace ProjetoTransactionApi.Controllers
{
    [Route("api/fund-transfer")]
    [ApiController]
    public class FundTransferController : ControllerBase
    {
        private readonly ITransferFundService _transferFundService;

        public FundTransferController(ITransferFundService transferFundService)
        {
            _transferFundService = transferFundService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FundTransferRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var result = await _transferFundService.ExecuteTransaction(request);
            return Created("Created",result);
        }

    }
}
