// <copyright file="IgnoredRoutesConfiguration.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Metrics.AspNetCore.Tracking
{
    internal class IgnoredRoutesConfiguration : IList<string>
    {
        private readonly List<string> _stringListImplementation;
        private readonly List<Regex> _regexList;

        public IgnoredRoutesConfiguration()
            : this(new List<string>())
        {
        }

        public IgnoredRoutesConfiguration(IList<string> patterns)
        {
            _stringListImplementation = patterns.ToList();
            _regexList = _stringListImplementation.Select(GetRegex).ToList();
        }

        public IReadOnlyList<Regex> RegexPatterns => _regexList;

        public IEnumerator<string> GetEnumerator() { return _stringListImplementation.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable)_stringListImplementation).GetEnumerator(); }

        public void Add(string item)
        {
            _regexList.Add(GetRegex(item));
            _stringListImplementation.Add(item);
        }

        public void Clear()
        {
            _regexList.Clear();
            _stringListImplementation.Clear();
        }

        public bool Contains(string item)
        {
            return _stringListImplementation.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _stringListImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            var index = _stringListImplementation.IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            _regexList.RemoveAt(index);
            _stringListImplementation.RemoveAt(index);

            return true;
        }

        public int Count => _stringListImplementation.Count;

        public bool IsReadOnly => false;

        public int IndexOf(string item)
        {
            return _stringListImplementation.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            _regexList.Insert(index, GetRegex(item));
            _stringListImplementation.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _regexList.RemoveAt(index);
            _stringListImplementation.RemoveAt(index);
        }

        public string this[int index]
        {
            get => _stringListImplementation[index];
            set
            {
                _stringListImplementation[index] = value;
                _regexList[index] = GetRegex(value);
            }
        }

        private static Regex GetRegex(string pattern) =>
            new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
