using System;

namespace OSPSuite.DataBinding
{
   public class PropertyValueSetEventArgs<TPropertyType> : EventArgs
   {
      public TPropertyType NewValue { get; }
      public TPropertyType OldValue { get; }
      public string PropertyName { get; }

      public PropertyValueSetEventArgs(string propertyName, TPropertyType oldValue, TPropertyType newValue)
      {
         PropertyName = propertyName;
         OldValue = oldValue;
         NewValue = newValue;
      }
   }
}