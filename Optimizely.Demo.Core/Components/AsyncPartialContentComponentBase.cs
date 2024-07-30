using EPiServer.Core;
using EPiServer.Web.Mvc;

namespace Optimizely.Demo.Core.Components;

public abstract class AsyncPartialContentComponentBase<TContentData> : AsyncPartialContentComponent<TContentData>
	where TContentData : IContentData
{ }
