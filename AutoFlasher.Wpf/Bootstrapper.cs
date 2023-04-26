﻿using AutoFlasher.Wpf.Components;
using AutoFlasher.Wpf.Interfaces;
using AutoFlasher.Wpf.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoFlasher.Wpf
{
    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        private AggregateCatalog aggregateCatalog;

        private IWindowManager windowManager;

        private IEventAggregator eventAggregator;

        private SimpleContainer simpleContainer;

        private ILoggerService loggerService;

        public Bootstrapper() {
            Initialize();
        }

        protected override void Configure()
        {

            //new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules"), "AutoFlasher.Modules.*.dll")
            //var aggregateCatalog = new AggregateCatalog(
            //    new AssemblyCatalog(Assembly.GetExecutingAssembly())
            //);

            aggregateCatalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());
            if (Directory.Exists(@"modules"))
                aggregateCatalog.Catalogs.Add(new DirectoryCatalog(@"modules", "Moudles.*.dll"));

            container = new CompositionContainer(aggregateCatalog);
            var batch = new CompositionBatch();

            windowManager = new WindowManager();
            batch.AddExportedValue(windowManager);

            eventAggregator = new EventAggregator();
            batch.AddExportedValue(eventAggregator);

            loggerService = new LoggerService();
            batch.AddExportedValue(loggerService);

            simpleContainer = new SimpleContainer();
            simpleContainer.Singleton<IViewModel, ShellViewModel>(Constants.ShellView);
            batch.AddExportedValue(simpleContainer);

            container.Compose(batch);
        }


        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await windowManager.ShowWindowAsync(simpleContainer.GetInstance<IViewModel>(Constants.ShellView));
        }

        protected override object GetInstance(Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key)
                ? AttributedModelServices.GetContractName(service)
                : key;

            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception($"找不到实例{contract}");

        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }
    }
}
