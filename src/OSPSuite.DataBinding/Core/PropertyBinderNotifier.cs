using System;
using System.ComponentModel;
using System.Reflection;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.DataBinding.Core
{
   public interface IPropertyBinderNotifier<TObjectType, TPropertyType> : IPropertyBinder<TObjectType, TPropertyType>
   {
      void AddValueChangedListener(TObjectType source, Action handler);
      void RemoveValueChangedListener(TObjectType source);
   }

   public class PropertyBinderNotifier<TObjectType, TPropertyType> : PropertyBinder<TObjectType, TPropertyType>, IPropertyBinderNotifier<TObjectType, TPropertyType>
   {
      private Action _handler;

      public PropertyBinderNotifier(PropertyInfo propertyInfo) : base(propertyInfo)
      {
      }

      public void AddValueChangedListener(TObjectType source, Action handler)
      {
         _handler = handler;
         //First try: Check if the object implements INotifyPropertyChanged
         var notifiable = source as INotifyPropertyChanged;
         if (notifiable != null)
         {
            notifiable.PropertyChanged += NotifiableEventHandler;
            return;
         }

         //Second try: Check if an event <PropertyName>Changed was defined and register to it
         var eventDescriptor = getEventDescriptor(source);
         if (eventDescriptor != null)
            eventDescriptor.AddEventHandler(source, EventDescriptorEventHandler());
      }

      public void RemoveValueChangedListener(TObjectType source)
      {
         //First try: Check if the object implements INotifyPropertyChanged
         var notifiable = source as INotifyPropertyChanged;
         if (notifiable != null)
         {
            notifiable.PropertyChanged -= NotifiableEventHandler;
            return;
         }

         //Second try: Check if an event <PropertyName>Changed was defined and register to it
         var eventDescriptor = getEventDescriptor(source);
         if (eventDescriptor != null)
         {
            eventDescriptor.RemoveEventHandler(source, EventDescriptorEventHandler());
            return;
         }
         _handler = null;

      }

      private EventHandler EventDescriptorEventHandler()
      {
         return (o, e) => _handler();
      }

      private void NotifiableEventHandler(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(PropertyName))
         {
            _handler();
         }
      }

      private EventDescriptor getEventDescriptor(TObjectType source)
      {
         var eventDescriptor = TypeDescriptor.GetEvents(source)[eventNameFromProperty()];
         if (eventDescriptor == null || eventDescriptor.EventType != typeof (EventHandler)) return null;
         return eventDescriptor;
      }

      private string eventNameFromProperty()
      {
         return string.Format("{0}Changed", PropertyName);
      }
   }
}