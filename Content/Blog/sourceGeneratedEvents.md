---
title: Source-generated event aggregation through interfaces
lead: Less boilerplate, faster runtime
published: 2026-10-26
tags: [C#, EventAggregator]
authors:
    - name: "CodeBin"
      gitHubUserName: "zhadowplague"
---


A common pattern for using an event aggregator looks something like this:

```csharp
public class Consumer : IDisposable
{
	IEventAggregator _agg;
	public Consumer(IEventAggregator agg)
	{
		_agg = agg;
		_agg.Subscribe<Message1>(OnMessage);
		_agg.Subscribe<Message2>(OnMessage);
	}

	private void OnMessage(Message1 msg) 
	{

	}
	
	private void OnMessage(Message2 msg) 
	{

	}

	public void Dispose()
	{
		_agg.Unsubscribe<Message1>(OnMessage)
		_agg.Unsubscribe<Message2>(OnMessage)
	}
}
```

Problems with this solution:
I don't like having to use classes as payloads.
I don't want to write a subscribe and unsubscribe for each individual event.
I want to be able to use ref structs and avoid reflection invocations.


Let me show you an alternative that solves these issues using a source generator:

```csharp
public class Consumer : IReceiver<Message1>, IReceiver<Message2>, IDispose
{
	IEventAggregator _agg;
	public Consumer(IEventAggregator agg)
	{
		_agg = agg;
		this.ListenToZEvents(agg);
	}

	internal void OnEvent(Message1 msg)
	{

	}

	internal void OnEvent(Message2 msg)
	{

	}

	public void Dispose()
	{
		this.FreeZEvents(_agg);
	}
}
```

The generated source code will look like this:

```csharp
public static class ConsumerExtensions
{
	public static void ListenToZEvents(this Consumer target, IEventAggregator bus)
	{
		bus.Listen<Message1>(target);
		bus.Listen<Message2>(target);
	}
	public static void FreeZEvents(this Consumer target, IEventAggregator bus)
	{
		bus.Free<Message1>(target);
		bus.Free<Message2>(target);
	}
}
```

You could also go a step further and use weak references in your EventAggregator to remove even more boilerplate:
```csharp
public class Consumer : IReceiver<Message1>, IReceiver<Message2>
{
	public Consumer(IEventAggregator agg)
	{
		_agg = agg;
		this.ListenToZEvents(agg);
	}

	internal void OnEvent(Message1 msg)
	{

	}

	internal void OnEvent(Message2 msg)
	{

	}
}
```

However, this means you must ensure the class returns early in an event receiver when it is supposed to be disposed.


The source generator for this example is published as an open-source NuGet package at: https://github.com/zhadowplague/ZEventAggregator
