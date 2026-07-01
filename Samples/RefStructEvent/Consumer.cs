namespace BlazorStaticMinimalBlog.Samples.RefStructEvent;

public class Consumer
{
	public void Consume(ImmutableEventData data)
	{
		//I can
		Console.WriteLine(data.Data[0]); //Use the data momentarily
		//I can
		var copy = data.Copy(); //Explicitly create a copy of it

		//However...

		//I cannot store it as a field
		//Nor access it outside of the public properties on the ImmutableEventData struct
	}

	//ImmutableEventData _data; <- Wont compile
}