namespace BlazorStaticMinimalBlog.Samples.RefStructEvent;

public readonly ref struct ImmutableEventData(byte[] data)
{
	private readonly byte[] _data = data;
	public Span<byte> Data => _data;
	public byte[] Copy()
	{
		var copy = new byte[_data.Length];
		_data.CopyTo(copy, 0);
		return copy;
	}
}
