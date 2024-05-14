

using MessagerWorker.Models;
using MessagerWorker.Queue;
using MessagerWorker;
using MessageWorker.Queue.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IMessageQueueSender<TransferEntity>, TransferEntityQueueSender>();
builder.Services.AddSingleton((sp) => {

    var transferQueueSetting = new TransferQueueSetting();
    builder.Configuration.GetSection(nameof(TransferQueueSetting)).Bind(transferQueueSetting);
    return transferQueueSetting;

}
);

var host = builder.Build();
host.Run();
