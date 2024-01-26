using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DialogPhrase
{
    public List<string> text;

    public List<string> correctOptions;
    
    public bool IsCorrectOption(Cassette.CassetteType option)
    {
        return correctOptions.Any(strOp => strOp.ToLower() == option.ToString().ToLower());
    }
}