using EPiServer.Core;
using EPiServer.PlugIn;
using Optimizely.Demo.Core.Models.Definitions;

namespace Optimizely.Demo.Core.Models.Properties;

[PropertyDefinitionTypePlugIn]
internal class NameValueStringListProperty : PropertyList<NameValueStringDefinition>
{ }
