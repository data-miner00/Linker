using System.Data;
using System.ServiceProcess;
using Autofac;
using Linker.WebJob;

var container = ContainerConfig.Configure();

var service = container.Resolve<WebJobService>();

var methodInfo = service.GetType().GetMethod("OnStart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
methodInfo.Invoke(service, new object[] { Array.Empty<string>() });

//ServiceBase.Run(service);

while (true)
{
    await Task.Delay(TimeSpan.FromSeconds(200));
}
