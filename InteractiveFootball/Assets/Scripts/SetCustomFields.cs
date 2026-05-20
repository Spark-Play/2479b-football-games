using TMPro;
using UnityEngine;

public class SetCustomFields : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    public void SetTextValue(string textString)
    {
        text.text = textString;
    }
}
