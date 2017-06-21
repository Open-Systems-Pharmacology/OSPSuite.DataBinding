using System;
using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Utility.Format;

namespace OSPSuite.DataBinding
{
   public interface IElementBinder<TObjectType> : ILatchable, IDisposable
   {
      /// <summary>
      /// Reference to parent binder
      /// </summary>
      ScreenBinder<TObjectType> ParentBinder { get; set; }

      /// <summary>
      /// Name of property to bind to
      /// </summary>
      string PropertyName { get; }

      /// <summary>
      /// Source object the element is binding to
      /// </summary>
      TObjectType Source { get; }

      /// <summary>
      /// Control to which the source property is bound to
      /// </summary>
      Control Control { get; }

      /// <summary>
      /// Performs the actual data binding to the source
      /// </summary>
      /// <param name="source">source object to bind to</param>
      void Bind(TObjectType source);

      /// <summary>
      /// Resets to the orignal value of the source
      /// </summary>
      void Reset();

      /// <summary>
      /// Updates the control with the object value
      /// </summary>
      /// <remarks>only relevant if the Data binding mode is set to deferred</remarks>
      void Update();

      /// <summary>
      /// Event is raised whenenver the value was changed, either in the element or if the source object itself
      /// </summary>
      event Action Changed;

      /// <summary>
      /// Event is raised when the value is changing in the element, typically via user input.
      /// </summary>
      event Action Changing;

      /// <summary>
      /// Validates current value in screen element for the bound property of the source object
      /// </summary>
      void Validate();

      /// <summary>
      /// Returns true if the current element binder contains an error otherwise false
      /// </summary>
      /// <returns>true if the current element binder contains an error otherwise false</returns>
      bool HasError { get; }

      /// <summary>
      /// Returns the current error message for the element binder if the binder has an error, 
      /// otherwise an empty string
      /// </summary>
      string ErrorMessage { get; }

      /// <summary>
      /// Removes binding to data source
      /// </summary>
      void DeleteBinding();
   }

   public interface IElementBinder<TObject, TPropertyType> : IElementBinder<TObject>
   {
      /// <summary>
      /// Returns value from the underlying source
      /// </summary>
      /// <returns>the actual value of the source for the bound property</returns>
      TPropertyType GetValueFromSource();

      /// <summary>
      /// Returns the value from the control
      /// </summary>
      /// <returns>the actual value of the control</returns>
      TPropertyType GetValueFromControl();

      /// <summary>
      /// Sets Value into control
      /// </summary>
      /// <param name="value">value to be set into the underlying control</param>
      void SetValueToControl(TPropertyType value);

      /// <summary>
      /// Sets Value into source
      /// </summary>
      /// <param name="value">value to be set into the underlying source</param>
      void SetValueToSource(TPropertyType value);

      /// <summary>
      /// Event will be raised before writing the value to the source so that a caller
      /// can take over the action source.Property = value (e.g. for undo/redo actions or protocols).
      /// The value will be set in the source from the control to ensure a bi-directional binding if using <see cref="BindingMode.TwoWay"/>.
      /// </summary>
      event Action<TObject, PropertyValueSetEventArgs<TPropertyType>> OnValueUpdating;

      /// <summary>
      /// Event will be raised once the value was set in the source object 
      /// The value will be set in the source from the control to ensure a bi-directional binding if using <see cref="BindingMode.TwoWay"/>.
      /// </summary>
      event Action<TObject, TPropertyType> OnValueUpdated;


      /// <summary>
      /// Formatter function used to display the value in a specific format
      /// </summary>
      IFormatterConfigurator<TObject, TPropertyType> FormatterConfigurator { get; set; }
   }
}