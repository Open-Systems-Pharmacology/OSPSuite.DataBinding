using System;
using OSPSuite.Utility.Format;

namespace OSPSuite.DataBinding
{
    public static class ElementBinderExtensions
    {
        public static IElementBinder<TObjectType, TProperty> WithFormat<TObjectType, TProperty>
            (this IElementBinder<TObjectType, TProperty> elementBinder, Func<TProperty, string> format)
        {
            return elementBinder.WithFormat(new DynamicFormatter<TProperty>(format));
        }

        public static IElementBinder<TObjectType, TProperty> WithFormat<TObjectType, TProperty>
            (this IElementBinder<TObjectType, TProperty> elementBinder, IFormatter<TProperty> formatter)
        {
            return elementBinder.WithFormat(source => formatter);
        }

        public static IElementBinder<TObjectType, TProperty> WithFormat<TObjectType, TProperty>
          (this IElementBinder<TObjectType, TProperty> elementBinder, Func<TObjectType, IFormatter<TProperty>> formatterProvider)
        {
           elementBinder.FormatterConfigurator = new FormatterConfigurator<TObjectType, TProperty>(formatterProvider);
           return elementBinder;
        }
    }
}