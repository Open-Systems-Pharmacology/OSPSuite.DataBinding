using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.DataBinding
{
   public enum SavingMode
   {
      /// <summary>
      ///    Saves the value into the source even if the element has an error
      /// </summary>
      Always,

      /// <summary>
      ///    Saves the value into the source only if the value in the control is valid
      /// </summary>
      OnlyWhenValid
   }

   public class ScreenBinder<TObjectType> : IBinder
   {
      //List of all screen elements maanaged by the binder
      private readonly IList<IElementBinder<TObjectType>> _allElements;

      public bool IsLatched { get; set; }

      public event Action Changed = delegate { };

      /// <summary>
      /// Event is fired whenever the value in one item is changing but has not be saved necessarily in the source
      /// </summary>
      public event Action Changing = delegate { };

      /// <summary>
      /// Event is fired whenever an error was found during the validation process
      /// </summary>
      public event Action<Control, string> OnValidationError = delegate { };

      /// <summary>
      /// Event is fired whenever the validation of an item was successful
      /// </summary>
      public event Action<Control> OnValidated = delegate { };
      public BindingMode BindingMode { get; set; }

      /// <summary>
      /// The SavingMode value determines when the data be saved in the source 
      /// Default value is OnlyWhenValid
      /// </summary>
      public SavingMode SavingMode { get; set; }

      public ScreenBinder() : this(new List<IElementBinder<TObjectType>>())
      {
      }

      internal ScreenBinder(IList<IElementBinder<TObjectType>> allElements)
      {
         _allElements = allElements;
         BindingMode = BindingMode.TwoWay;
         SavingMode = SavingMode.OnlyWhenValid;
      }

      /// <summary>
      /// Adds a screen element to the the screen binder
      /// </summary>
      /// <param name="elementBinder">screen element to add</param>
      public void AddElement(IElementBinder<TObjectType> elementBinder)
      {
         _allElements.Add(elementBinder);
         elementBinder.Changed += () => Changed();
         elementBinder.Changing += () => Changing();
         elementBinder.ParentBinder = this;
      }

      /// <summary>
      /// Binds the screen to the given source
      /// </summary>
      /// <param name="source">Source object to bind to</param>
      public void BindToSource(TObjectType source)
      {
         //Fisrt bind to the source
         this.DoWithinLatch(() => _allElements.Each(element => element.Bind(source)));

         //then validate all elements at once
         Validate();
      }

      public bool HasError
      {
         get { return _allElements.Any(element => element.HasError); }
      }

      public IEnumerable<string> ErrorMessages => from e in _allElements
         let error = e.ErrorMessage
         where !string.IsNullOrEmpty(error)
         select error;

      public string ErrorMessage => ErrorMessages.ToString("\n");

      /// <summary>
      /// Clears error for the screen element given as parameter
      /// </summary>
      /// <param name="elementBinder">element for which the error provider should be cleared</param>
      public void ClearError(IElementBinder<TObjectType> elementBinder)
      {
         this.DoWithinLatch(() => OnValidated(elementBinder.Control));
      }

      /// <summary>
      /// Sets error for the screen element given as parameter
      /// </summary>
      /// <param name="elementBinder">element for which the error provider should be set</param>
      /// <param name="errorMessage">Error message to be displayed by the error provider</param>
      public void SetError(IElementBinder<TObjectType> elementBinder, string errorMessage)
      {
         this.DoWithinLatch(() => OnValidationError(elementBinder.Control, errorMessage));
      }

      /// <summary>
      /// Removes the element binder from the screen binder. This should only be used in extrem scenarios
      /// This should be called before the bind methods
      /// </summary>
      /// <param name="elementBinder">Screen element to remove</param>
      public void Remove(IElementBinder<TObjectType> elementBinder)
      {
         _allElements.Remove(elementBinder);
      }

      /// <summary>
      /// Refreshes the content of all list items(e.g update the combo box list)
      /// </summary>
      public void RefreshListElements()
      {
         var query = from element in _allElements
                     let listElement = element as IListElementBinder<TObjectType>
                     where listElement != null
                     select listElement;

         this.DoWithinLatch(() => query.Each(listElement => listElement.Refresh()));
      }

      /// <summary>
      /// Validates all avaialble elements
      /// </summary>
      public void Validate()
      {
         _allElements.Each(element => element.Validate());
      }

      /// <summary>
      /// Resets all screen element to the original value of the source when BindToSource was called
      /// </summary>
      public void Reset()
      {
         _allElements.Each(element => element.Reset());
      }

      /// <summary>
      /// Updates all screen element with the current value of the source
      /// </summary>
      public void Update()
      {
         this.DoWithinLatch(() => _allElements.Each(element => element.Update()));
      }

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      private void cleanup()
      {
         DeleteBinding();
         _allElements.Each(element => element.Dispose());
         _allElements.Clear();
      }

      public void DeleteBinding()
      {
         _allElements.Each(element => element.DeleteBinding());
      }

      ~ScreenBinder()
      {
         cleanup();
      }
   }
}