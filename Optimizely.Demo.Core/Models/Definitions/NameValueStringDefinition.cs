using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.Core.Models.Definitions;

public class NameValueStringDefinition
{
    public virtual string Name { get; set; }

    [UIHint(UIHint.Textarea)]
    public virtual string Value { get; set; }
}