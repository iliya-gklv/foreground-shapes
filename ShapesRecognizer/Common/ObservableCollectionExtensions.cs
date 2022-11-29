using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShapesRecognizer.Common
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> list)
        {
            foreach (var value in list)
            {
                collection.Add(value);
            }
        }
        
        
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            var itemsToDelete = collection.Where(predicate).ToList();
            
            foreach (var value in itemsToDelete)
            {
                collection.Remove(value);
            }
        }
    }
}