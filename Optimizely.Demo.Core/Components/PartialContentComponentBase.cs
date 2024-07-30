using EPiServer.Core;
using EPiServer.Web.Mvc;

namespace Optimizely.Demo.Core.Components;

public abstract class PartialContentComponentBase<TContentData> : PartialContentComponent<TContentData>
	where TContentData : IContentData
{ }
