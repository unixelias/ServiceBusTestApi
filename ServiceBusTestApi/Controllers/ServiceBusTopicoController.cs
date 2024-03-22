using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ServiceBusTestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceBusTopicoController : ControllerBase
    {
        private readonly ILogger<ServiceBusTopicoController> _logger;

        public ServiceBusTopicoController(ILogger<ServiceBusTopicoController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Envia mensagens para um t�pico do Service Bus
        /// </summary>
        /// <param name="mensagens">Mensagens a serem enviadas ao t�pico</param>
        /// <param name="nomeTopico">Nome do t�pico</param>
        /// <param name="connectionString">String de conex�o com o Service Bus</param>
        /// <returns>Retorna uma mensagem de sucesso ou erro</returns>

        [HttpPost("{nomeTopico}")]
        public async Task<IActionResult> EnviaMensagensParaServiceBus([FromBody] object mensagens, [FromRoute] string nomeTopico, [FromHeader] string connectionString)
        {
            await using var client = new ServiceBusClient(connectionString);

            if (nomeTopico.Contains("prd"))
            {
                return BadRequest("N�o � permitido enviar mensagens para o ambiente de produ��o");
            }
            ServiceBusSender sender = client.CreateSender(nomeTopico);

            string dados = JsonSerializer.Serialize(mensagens);
            _logger.LogInformation("Mensagem: {dados}", dados);
            var mensagem = new ServiceBusMessage(dados);

            try
            {
                await sender.SendMessageAsync(mensagem);
                return Ok($"Mensagem enviada com sucesso ao t�pico {nomeTopico}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}