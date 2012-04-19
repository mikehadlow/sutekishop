using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Suteki.Shop.Tests
{
	public class FakeValueProvider : IValueProvider, IDictionary<string, ValueProviderResult> 
	{
		private readonly Dictionary<string, ValueProviderResult> innerDictionary = new Dictionary<string, ValueProviderResult>();

		public void AddValue(string key, object rawValue, string attemptedValue)
		{
			Add(key, new ValueProviderResult(rawValue, attemptedValue, CultureInfo.CurrentCulture));
		}


		private ICollection<KeyValuePair<string, ValueProviderResult>> AsCollection
		{
			get { return innerDictionary; }
		}

		public IEnumerator<KeyValuePair<string, ValueProviderResult>> GetEnumerator()
		{
			return innerDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return innerDictionary.GetEnumerator();
		}

		public void Add(KeyValuePair<string, ValueProviderResult> item)
		{
			AsCollection.Add(item);
		}

		public void Clear()
		{
			innerDictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, ValueProviderResult> item)
		{
			return AsCollection.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, ValueProviderResult>[] array, int arrayIndex)
		{
			AsCollection.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, ValueProviderResult> item)
		{
			return AsCollection.Remove(item);
		}

		public int Count
		{
			get { return innerDictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return AsCollection.IsReadOnly; }
		}

		public bool ContainsKey(string key)
		{
			return innerDictionary.ContainsKey(key);
		}

		public void Add(string key, ValueProviderResult value)
		{
			innerDictionary.Add(key,value);
		}

		public bool Remove(string key)
		{
			return innerDictionary.Remove(key);
		}

		public bool TryGetValue(string key, out ValueProviderResult value)
		{
			return innerDictionary.TryGetValue(key, out value);
		}

		public ValueProviderResult this[string key]
		{
			get
			{
				ValueProviderResult result;
				innerDictionary.TryGetValue(key, out result);
				return result;
			}
			set { innerDictionary[key] = value; }
		}

		public ICollection<string> Keys
		{
			get { return innerDictionary.Keys; }
		}

		public ICollection<ValueProviderResult> Values
		{
			get { return innerDictionary.Values; }
		}

	    public bool ContainsPrefix(string prefix)
	    {
	        Console.WriteLine("ContainsPrefix: {0}", prefix);
	        return false;
	    }

	    public ValueProviderResult GetValue(string key)
	    {

	        return this[key];
	    }
	}
}