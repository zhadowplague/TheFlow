namespace BlazorStaticMinimalBlog.Samples.RefStructEvent;

public class Producer
{
	public void Produce()
	{
		using var data = new RentedArray<byte>(10000);

		ParseSocketFileOrWhathever(data);

		Publish.Event(data);
	}

	private static void ParseSocketFileOrWhathever(RentedArray<byte> toFill)
	{
		for (int i = 0; i < toFill.Length; i++)
		{
			toFill.Array[i] = (byte)i;
		}
	}
}