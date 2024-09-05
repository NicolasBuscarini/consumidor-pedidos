using System.Diagnostics.CodeAnalysis;

namespace ConsumidorPedidos.Tests.Utility
{
    [ExcludeFromCodeCoverage]
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        public T Current => _innerEnumerator.Current;

        public ValueTask DisposeAsync()
        {
            _innerEnumerator.Dispose();
            return new ValueTask();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_innerEnumerator.MoveNext());
        }
    }
}
