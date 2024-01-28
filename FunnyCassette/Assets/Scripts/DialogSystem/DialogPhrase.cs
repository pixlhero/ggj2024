using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

[Serializable]
public class DialogPhrase
{
    public List<string> text;

    public List<string> correctOptions;
    
    public string hahaReaction;
    
    public string dotdotdotReaction;
    
    public string ohnoReaction;
    
    public string next;
    
    public bool IsCorrectOption(Cassette.CassetteType option)
    {
        return correctOptions.Any(strOp => strOp.ToLower() == option.ToString().ToLower());
    }
    
    public string GetReaction(Cassette.CassetteType option)
    {
        switch (option)
        {
            case Cassette.CassetteType.Haha:
                return hahaReaction;
            case Cassette.CassetteType.DotDotDot:
                return dotdotdotReaction;
            case Cassette.CassetteType.OhNo:
                return ohnoReaction;
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
        }
    }
}