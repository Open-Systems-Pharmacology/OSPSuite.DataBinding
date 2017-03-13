using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding
{
   public interface IListElementBinder<TObject> : IElementBinder<TObject>
   {
      /// <summary>
      ///    Refresh the content of the supervised list control
      /// </summary>
      void Refresh();
   }

   public interface IListElementBinder<TObject, TPropertyType> : IListElementBinder<TObject>, IElementBinder<TObject, TPropertyType>
   {
      /// <summary>
      ///    List of available values to bind to
      /// </summary>
      Func<TObject, IEnumerable<TPropertyType>> ListOfValues { get; set; }

      /// <summary>
      ///    List of display values
      /// </summary>
      Func<TObject, IEnumerable<string>> ListOfDisplayValues { get; set; }

      /// <summary>
      ///    One display value for one item in the list
      /// </summary>
      Func<TPropertyType, string> DisplayValueFor { get; set; }
   }

   public abstract class ListElementBinder<TObject, TPropertyType> : ElementBinder<TObject, TPropertyType>, IListElementBinder<TObject, TPropertyType>
   {
      private IList<TPropertyType> _listOfValues;
      private IList<string> _listOfDisplayValues;

      protected ListElementBinder(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder) : base(propertyBinder)
      {
      }

      public Func<TObject, IEnumerable<TPropertyType>> ListOfValues { get; set; }
      public Func<TObject, IEnumerable<string>> ListOfDisplayValues { get; set; }
      public Func<TPropertyType, string> DisplayValueFor { get; set; }

      protected abstract void FillWith(IEnumerable<TPropertyType> listOfValues, IEnumerable<string> listOfDisplayValues);

      protected override void UpdateSource(TObject source)
      {
         base.UpdateSource(source);
         createListsFrom(source);
         FillWith(_listOfValues, _listOfDisplayValues);
      }

      public void Refresh()
      {
         createListsFrom(Source);
         FillWith(_listOfValues, _listOfDisplayValues);
         SetValueToControl(GetValueFromSource());

         //if the index was not found, it might be because we had a combo box refresh that resulted into an invalid value 
         //we need to signal a value in control changed

         ValueInControlChanged();
      }

      protected int IndexFromValue(TPropertyType value)
      {
         return _listOfValues.IndexOf(value);
      }

      protected TPropertyType ValueFromDisplayItem(string displayItem)
      {
         return ValueFromIndex(_listOfDisplayValues.IndexOf(displayItem));
      }

      protected TPropertyType ValueFromIndex(int index)
      {
         if (index < 0 || index >= _listOfValues.Count)
            return default(TPropertyType);

         return _listOfValues[index];
      }

      private void createListsFrom(TObject source)
      {
         if (ListOfValues == null)
            throw new ArgumentException("ListOfValues is not set.");

         _listOfValues = ListOfValues(source).ToList();

         setDisplayValuesFrom(source);

         if (_listOfValues.Count == _listOfDisplayValues.Count) return;

         throw new ArgumentException("Number of items in the 'Key' list and the 'Display' list are not equal.");
      }

      private void setDisplayValuesFrom(TObject source)
      {
         if (noDisplayValuesWereDefined())
            _listOfDisplayValues = _listOfValues.Select(item => Formatter.Format(item)).ToList();

         else if (ListOfDisplayValues != null)
            _listOfDisplayValues = ListOfDisplayValues(source).ToList();

         else
         {
            _listOfDisplayValues = new List<string>();
            _listOfValues.Each(value => _listOfDisplayValues.Add(DisplayValueFor(value)));
         }
      }

      private bool noDisplayValuesWereDefined()
      {
         return ListOfDisplayValues == null && DisplayValueFor == null;
      }

      public override void DeleteBinding()
      {
         base.DeleteBinding();
         _listOfValues?.Clear();
         _listOfDisplayValues?.Clear();
      }

      protected override void Cleanup()
      {
         try
         {
            _listOfDisplayValues = null;
            _listOfValues = null;
         }
         finally
         {
            base.Cleanup();
         }
      }
   }
}