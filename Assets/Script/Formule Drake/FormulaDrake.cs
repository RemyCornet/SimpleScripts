using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FormulaDrake : MonoBehaviour
{
    public InputField Rxtext;
    public InputField fptext;
    public InputField netext;
    public InputField fltext;
    public InputField fitext;
    public InputField fctext;
    public InputField Ltext;

    public Text Resultat;

    string Erreur = "erreur";
    
    public void Calcule()
    {
        if ((Rxtext.text != "") && (fptext.text != "") && (netext.text != "") && (fltext.text != "") && (fitext.text != "") && (fctext.text != "") && (Ltext.text != ""))
        {
           float Rx = float.Parse(Rxtext.text, CultureInfo.InvariantCulture);
           float fp = float.Parse(fptext.text, CultureInfo.InvariantCulture);
           float ne = float.Parse(netext.text, CultureInfo.InvariantCulture);
           float fl = float.Parse(fltext.text, CultureInfo.InvariantCulture);
           float fi = float.Parse(fitext.text, CultureInfo.InvariantCulture);
           float fc = float.Parse(fctext.text, CultureInfo.InvariantCulture);
           float L = float.Parse(Ltext.text, CultureInfo.InvariantCulture);
           float resultat = Rx * fp * ne * fl * fi * fc * L;

            Resultat.text = resultat.ToString();
        }
        else
        {
            Resultat.text = Erreur;
        } 
    }
}
