using System;
using System.Collections.Generic;

namespace log4net.loggly
{
	public static class DictionaryOfListsExtensions
	{
		public static Dictionary<TKey, List<TElement>> ToDictionaryOfLists<TKey, TElement>(
			this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
		{
			return ToDictionaryOfLists(source, keySelector, value => value);
		}

		public static Dictionary<TKey, List<TElement>> ToDictionaryOfLists<TKey, TSource, TElement>(
			this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
			Func<TSource, TElement> elementSelector)
		{
			var valueDict = new Dictionary<TKey, List<TElement>>();
			foreach (var value in source)
			{
				var key = keySelector(value);

				List<TElement> valueList;
				if (!valueDict.TryGetValue(key, out valueList))
				{
					valueList = new List<TElement>();
					valueDict.Add(key, valueList);
				}
				valueList.Add(elementSelector(value));
			}
			return valueDict;
		}
	}
}