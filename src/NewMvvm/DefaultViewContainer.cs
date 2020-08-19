﻿
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewMvvm
{
    internal class DefaultViewContainer : IViewContainer, IViewLocator
    {
        private readonly Dictionary<Type, RegisterInfo> map;

        public DefaultViewContainer()
        {
            map = new Dictionary<Type, RegisterInfo>();
        }

        internal DefaultViewContainer(IEnumerable<RegisterInfo> tuples)
        {
            map = tuples.ToDictionary(x => x.ViewModelType, x => x);
        }

        public Type GetView(Type vmType)
        {
            if (!map.TryGetValue(vmType, out var info))
            {
                throw new KeyNotFoundException(
                    string.Format(Properties.Resource.CannotFindMatchedViewTypeOfFormatString,
                    vmType.Name));
            }

            return viewType;
        }

        public void Register<TView, TViewModel>() where TView : class
        {
            Register(typeof(TViewModel), typeof(TView));
        }

        private void Register(Type viewModelType, Type viewType)
        {
            map[viewModelType] = viewType;
        }
    }

    internal class DefaultViewLocator : IViewInstanceLocator
    {
        private readonly IViewLocator viewLocator;
        private readonly IServiceProvider serviceProvider;

        public DefaultViewLocator(
            IViewLocator viewLocator,
            IServiceProvider serviceProvider)
        {
            this.viewLocator = viewLocator;
            this.serviceProvider = serviceProvider;
        }

        public object GetView(Type type)
        {
            return serviceProvider.GetService(viewLocator.GetView(type));
        }
    }
}
