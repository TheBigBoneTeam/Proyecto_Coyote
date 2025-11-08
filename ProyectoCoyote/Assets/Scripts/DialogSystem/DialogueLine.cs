using System.Collections.Generic;
using UnityEngine;
public class DialogueLine
{
    public string character;
    public List<string> textLines;

    public DialogueLine(string character, List<string> textLines)
    {
        this.character = character;
        this.textLines = textLines;
    }
}
