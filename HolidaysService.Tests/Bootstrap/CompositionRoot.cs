using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace HolidaysService.Tests.Bootstrap
{
    internal static class CompositionRoot
    {
        public static IServiceProvider ServiceProvider => ServiceCollection.BuildServiceProvider();
        public static IServiceCollection ServiceCollection => LazyContainer.InternalServiceCollection;

        [UsedImplicitly]
        private class LazyContainer
        {
            static LazyContainer()
            {
                InternalServiceCollection = new ServiceCollection()
                    .RegisterServices();
            }

            internal static readonly IServiceCollection InternalServiceCollection;
        }
    }
}