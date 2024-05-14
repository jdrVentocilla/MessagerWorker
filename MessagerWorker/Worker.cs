using Bogus;
using MessagerWorker.Models;
using MessagerWorker.Queue;


namespace MessagerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageQueueSender<TransferEntity> _messageSender;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger , 
                      IMessageQueueSender<TransferEntity> messageSender,
                      IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messageSender = messageSender;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var faker = new Faker<TransferEntity>()
                                    .RuleFor(x => x.Id, f => f.Random.AlphaNumeric(25))
                                    .RuleFor(x => x.NCuentaDestino, f => f.Finance.Account())
                                    .RuleFor(x => x.NCuentaOrigen, f => f.Finance.Account())
                                    .RuleFor(x => x.BancoOrigen, f => f.PickRandom(Others.Utils.GetExternalBanks()))
                                    .RuleFor(x => x.BancoDestino, f => f.PickRandom(Others.Utils.GetBank()))
                                    .RuleFor(x => x.FechaOperacion, f => DateTime.Now)
                                    .RuleFor(x => x.Monto, f => f.Random.Number(10000, 999999999))
                                    .RuleFor(x => x.TMovimiento, f => f.PickRandom(new[] { "Cargo", "Abono" }))
                                    .Generate();


                        _messageSender.Execute(faker);

                    }

                    

                }
                catch (Exception ex)
                {

                    _logger.LogInformation("Error al ejecutar la tarea. Detalles {0}.", ex.Message);
                }

                await Task.Delay(5000, stoppingToken);
            }   
        }
    }
}
