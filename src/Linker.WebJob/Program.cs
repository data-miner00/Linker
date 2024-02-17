using System.Data;
using System.ServiceProcess;
using Autofac;
using Linker.WebJob;
using Microsoft.Extensions.Hosting;

var container = ContainerConfig.Configure();

var service = container.Resolve<IHostedService>();

var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);

await service.StartAsync(cancellationTokenSource.Token).ConfigureAwait(false);

while (!cancellationTokenSource.IsCancellationRequested)
{
    await Task.Delay(TimeSpan.FromSeconds(200));
}

await service.StopAsync(default).ConfigureAwait(false);

Console.WriteLine("Program stopped...");

void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("CTRL+C pressed, cancelling operation");
    cancellationTokenSource.Cancel();

    e.Cancel = true;
}
