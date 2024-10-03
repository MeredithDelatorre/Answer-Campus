
using System.Collections.Generic;

[System.Serializable]
public class TextMessage : System.IEquatable<TextMessage>
{
    public Character from;
    public string message;
    public string location;

    public TextMessage[] positiveResponseBranch;
    public TextMessage[] negativeResponseBranch;

    public TextMessage(Character from, string message, string location)
    {
        this.from = from;
        this.message = message;
        this.location = location;
        //positiveResponseBranch = new List<TextMessage>();
        //negativeResponseBranch = new List<TextMessage>();
    }

    public TextMessage GetNextMessage(bool isPositiveResponse)
    {
        if (isPositiveResponse && positiveResponseBranch.Length > 0)
        {
            return positiveResponseBranch[0];
        }
        else if (!isPositiveResponse && negativeResponseBranch.Length > 0)
        {
            return negativeResponseBranch[0];
        }
        return null; // This is allowed because classes can be null
    }

    // Equality checks
    public bool Equals(TextMessage other)
    {
        return from == other.from && message == other.message && location == other.location;
    }

    public override bool Equals(object obj)
    {
        if (obj is TextMessage otherMessage)
            return Equals(otherMessage);

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + from.GetHashCode();
            hash = hash * 23 + (message?.GetHashCode() ?? 0);
            hash = hash * 23 + (location?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
