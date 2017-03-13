using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.DataBinding
{
    public static class ScreenBinderExtensions
    {
        public static IScreenToElementBinder<TObject,TPropertyType> Bind<TObject,TPropertyType>(
                this ScreenBinder<TObject> screenBinder,
                Expression<Func<TObject, TPropertyType>> propertyToBindTo)
        {
            var propertyInformation = new ExpressionInspectorFactory().Create<TObject>().PropertyFor(propertyToBindTo);
            return new ScreenToElementBinder<TObject, TPropertyType>(screenBinder,propertyInformation);

        }
    }
}