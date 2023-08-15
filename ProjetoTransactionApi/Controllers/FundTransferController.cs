using Azure.Core;
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
        private readonly ILogger<FundTransferController> _logger;

        public FundTransferController(ITransferFundService transferFundService, ILogger<FundTransferController> logger)
        {
            _transferFundService = transferFundService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FundTransferRequestDto request)
        {
            _logger.LogInformation($"{DateTime.Now} | Iniatiate transaction");
            if (!ModelState.IsValid)
                return BadRequest(request);

            var result = await _transferFundService.ExecuteTransaction(request);

            _logger.LogInformation($"{DateTime.Now} | Ending transaction");
            return Created("Created",result);
        }

        [HttpGet("{transactionId}")]
        public async Task<ActionResult> Get(Guid transactionId)
        {
            _logger.LogInformation($"{DateTime.Now} | Iniatiate consultation for transactionId");

            var result = await _transferFundService.GetStatusByTransactionId(transactionId);

            _logger.LogInformation($"{DateTime.Now} | Ending consultation for transactionId");
            return Created("Created", result);

        }

    }
}
