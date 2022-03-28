﻿using Autofac;

namespace Linker.ConsoleUI
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using var scope = container.BeginLifetimeScope();
            var app = scope.Resolve<IStartup>();
            app.Run();
        }
    }
}
