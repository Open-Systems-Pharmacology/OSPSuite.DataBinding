using System;
using System.Collections.Generic;

namespace OSPSuite.DataBinding
{
    public static class ListElementBinderExtensions
    {
        /// <summary>
        /// Binds the element binder to a static list of element
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfValues">static list of values</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            WithValues<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,IEnumerable<TPropertyType> listOfValues)
        {
            listElementBinder.ListOfValues = item => listOfValues;
            return listElementBinder;
        }

        /// <summary>
        /// Binds the element binder to a static list of element defined as a parameter array
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfValues">static list of values defined as a parameter array</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            WithValues<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,params TPropertyType[] listOfValues)
        {
            listElementBinder.ListOfValues = item => listOfValues;
            return listElementBinder;
        }


        /// <summary>
        /// Binds the element binder to a dymanic list of element defined as a function of the source object to bind to
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfValues">Delegate that takes the source object to bind to as parameter and returns the list of values </param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            WithValues<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,Func<TObject, IEnumerable<TPropertyType>> listOfValues)
        {
            listElementBinder.ListOfValues = listOfValues;
            return listElementBinder;
        }

        /// <summary>
        /// Defines the list of items to be displayed as a static enumeration of striong
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfDisplayValues">Static enumeration of strings</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            AndDisplays<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,IEnumerable<string> listOfDisplayValues)
        {
            listElementBinder.ListOfDisplayValues = item => listOfDisplayValues;
            return listElementBinder;
        }

        /// <summary>
        /// Defines the list of items to be displayed as a static enumeration of string  defined as a parameter array
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfDisplayValues">Static enumeration of strings defined as a parameter array</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            AndDisplays<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,params string[] listOfDisplayValues)
        {
            listElementBinder.ListOfDisplayValues = item => listOfDisplayValues;
            return listElementBinder;
        }


        /// <summary>
        /// Defines the list of items to be displayed as a dynamically as a function of the source object.
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="listOfDisplayValues">Delegate that takes the source object to bind to as parameter and returns the list of display values</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
            AndDisplays<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,Func<TObject, IEnumerable<string>> listOfDisplayValues)
        {
            listElementBinder.ListOfDisplayValues = listOfDisplayValues;
            return listElementBinder;
        }

        /// <summary>
        /// Defines the list of items to be displayed dynamically as a function of the object defined in the value list.
        /// </summary>
        /// <param name="listElementBinder">element binder</param>
        /// <param name="displayValueFor">Delegate that takes one object from the value list and returns a display for that object</param>
        /// <returns>the element binder</returns>
        public static IListElementBinder<TObject, TPropertyType>
           AndDisplays<TObject, TPropertyType>(this IListElementBinder<TObject, TPropertyType> listElementBinder,Func<TPropertyType, string> displayValueFor)
        {
            listElementBinder.DisplayValueFor = displayValueFor;
            return listElementBinder;
        }
    }
}