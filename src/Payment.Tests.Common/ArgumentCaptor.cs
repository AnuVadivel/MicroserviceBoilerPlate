using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;


namespace Payment.Tests.Common
{
    [ExcludeFromCodeCoverage]
    public class ArgumentCaptor<T>
    {
        public T Value { get; private set; }

        public List<T> Values { get; } = new();

        public T Capture()
        {
            return It.Is<T>(t => SaveValue(t));
        }

        private bool SaveValue(T t)
        {
            Value = t;
            Values.Add(t);
            return true;
        }
    }
}