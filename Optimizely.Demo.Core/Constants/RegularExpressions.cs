namespace Optimizely.Demo.ContentTypes.Constants;

public class RegularExpressions
{
    public const string PhoneRegexString = @"^\s*\+?\s*([0-9][\s-]*){7,}$";
    public const string GuidRegexString = @"\b[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b";
}
