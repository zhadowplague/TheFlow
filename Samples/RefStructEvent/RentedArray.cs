using System.Buffers;

namespace BlazorStaticMinimalBlog.Samples.RefStructEvent;

public readonly struct RentedArray<T>(int size) : IDisposable
{
	readonly T[] _arr = ArrayPool<T>.Shared.Rent(size);
	readonly int _size = size;

	public readonly int Length => _size;

	public readonly T[] Array => _arr;

	public readonly void Dispose()
	{
		ArrayPool<T>.Shared.Return(_arr);
	}
}
