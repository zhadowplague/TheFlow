---
title: Borrowed data as an event payload
lead: Restricting access to exposed borrowed data
published: 2026-06-26
tags: [C#, EventAggregator, ref struct]
authors:
    - name: "CodeBin"
      gitHubUserName: "zhadowplague"
---


The event aggregator pattern is a common thing in programs. One part in the app publishes data, and some other part listens to it. Both parts do not know each other.
The payload is commonly a reference type (i.e., a class) and as it is usually shared between consumers, it is not uncommon for it to be immutable in some way.

This is a very unrestrictive way to present data to consuming code. The consumer could store the reference in some form of buffer or cache. 
Or it could create a copy to ensure no one else is using the same data.

But what if the data is **large**? Or what if it's not thread-safe to run operations on it?
"That just means it's not supposed to be an event in the first place" you say?
Well... let me present an alternative.


## The ref struct type wrapper:

```csharp
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
```

### Pros
- Immutable
- Explicit syntax
### Cons
- You tell me

## Usage

Let us look at how the publisher could use it to save memory:

```csharp
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
```

Here we are utilizing a RentedArray<T>. A struct which uses the shared System.ArrayPool to temporarily allocate memory.

The RentedArray struct implements IDisposable so we won't have to think about how to rent the array. We just have to remember to dispose it. This is easily achieved **using** the using statement :v)

```csharp
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
```

And this is how the consumer would consume it:

```csharp
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
```
