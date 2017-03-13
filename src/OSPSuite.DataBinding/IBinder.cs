using System;
using System.Collections.Generic;
using OSPSuite.Utility;

namespace OSPSuite.DataBinding
{
   /// <summary>
   /// Defines how the binder will bind to the source
   /// </summary>
   public enum BindingMode
   {
      /// <summary>
      /// Changes to the source will update the target
      /// </summary>
      OneWay,

      /// <summary>
      /// Changes to the source will update the target and changes on the target will update the source
      /// </summary>
      TwoWay
   }

   public interface IBinder : ILatchable, IDisposable
   {
      /// <summary>
      /// The BindingMode value determines how the data will flow between the source and the target
      /// Default value is TwoWay
      /// </summary>
      BindingMode BindingMode { get; set; }

      /// <summary>
      /// Event is fired whenever a value in one of the element changed
      /// either manually (user input) or automatically (control update due to databinding)
      /// Event is fired after if the validation process was successful
      /// </summary>
      event Action Changed;

      /// <summary>
      /// Returns true if the bound element contains error. Otherwise false
      /// </summary>
      bool HasError { get; }

      /// <summary>
      /// Removes current binding to data source. Binding configuration remains unchanged
      /// </summary>
      void DeleteBinding();

      /// <summary>
      /// Returns the overall error messages that could be displayed to the user.
      /// The returned value should be an empty enumeration if the HasError returns false;
      /// Otherwise, one entry for each element with an error
      /// </summary>
      IEnumerable<string> ErrorMessages { get; }

      /// <summary>
      /// Returns the overall error messages in one string unsing a \n delimiter
      /// </summary>
      string ErrorMessage { get; }
   }
}