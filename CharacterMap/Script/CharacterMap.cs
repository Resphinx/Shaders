using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderTyping
{
    public class CharacterMap : MonoBehaviour
    {
        int maxCharacter = 512;
        public string allCharacters;
        [TextAreaAttribute(5, 10)]
        public string text;
        public string color;
        string lastColor;
        public bool wrap = false;
        string lastText;
        static string ascii = " \t!\"#$%'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnoprstuvwxyz{|}~";
        static string color36 = "0123456789abcdefghijklmnopqrstuvwxyz";
        string characterSet;
        public Texture2D colorMap;
        public Material material;
        public SpecialColors[] special;
        int paperWidth;
        Texture2D textTexture;
        Color32[] pixels;
        // Start is called before the first frame update
        void Start()
        {
            textTexture = (Texture2D)material.GetTexture("_Text");
            maxCharacter = textTexture.width;
            Debug.Log(maxCharacter + "," + paperWidth);
            pixels = new Color32[maxCharacter];
            lastText = text;
            lastColor = color;
            characterSet = allCharacters == "" ? ascii : allCharacters;
            WriteTexture();
        }

        // Update is called once per frame
        void Update()
        {
            if (text != lastText || color != lastColor)
            {
                lastText = text;
                lastColor = color;
                WriteTexture();
            }
            material.SetFloat("_ActiveTime", Time.time);

        }
        List<string> Words()
        {
            string whole = "";
            string wrapChars = " ,;:+-*/\n";
            List<string> words = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                if (wrapChars.IndexOf(text[i]) >= 0)
                {
                    if (whole.Length > 0)
                    {
                        words.Add(whole);
                        whole = "";
                    }
                    words.Add(text[i] + "");
                }
                else whole += text[i];
            }
            if (whole.Length > 0) words.Add(whole);
            return words;
        }
        bool Special(char c, out Color color)
        {

            foreach (SpecialColors sc in special)
                if (sc.chars.IndexOf(c) >= 0)
                {
                    color = sc.color;
                    return true;
                }
            color = Color.red;
            return false;
        }
        void WriteTexture()
        {
            Vector2 v = material.GetVector("_PaperSize");
            paperWidth = Mathf.RoundToInt(v.x);
            Color colorID, fontColor;
            int time;
            List<string> words = wrap ? Words() : new List<string>() { text };
            int lastColor = 0, colorIndex;
            int paperIndex = 0, textIndex = 0;
            int caret = 0;
            for (int w = 0; w < words.Count; w++)
            {
                if (wrap)
                    if (words[w].Length + caret >= paperWidth)
                    {
                        for (int j = 0; j < paperWidth - caret; j++)
                            textTexture.SetPixel(paperIndex++, 0, Color.black);
                        caret = 0;
                    }
                for (int i = 0; i < words[w].Length; i++)
                    if (words[w] == "\n")
                    {
                        for (int j = 0; j < paperWidth - caret; j++)
                            textTexture.SetPixel(paperIndex++, 0, Color.black);
                        caret = 0;
                    }
                    else
                    {
                        if (paperIndex < maxCharacter)
                        {
                            int index = characterSet.IndexOf(words[w][i]);
                            if (index < 0) textTexture.SetPixel(paperIndex, 0, Color.black);
                            else
                            {
                                byte q = (byte)((index % 25) * 10.2f);
                                float g = q / 255f + 0.01f;
                                q = (byte)((index / 25) * 10.2f);
                                float r = q / 255f + 0.01f;
                                colorID = new Color(r, g, 0, 1);
                           //     Debug.Log(words[w][i] + " "+paperIndex + " created: " + r + ", "+g);
                                if (color.Length > textIndex)
                                    if ((colorIndex = color36.IndexOf(color[textIndex])) >= 0) lastColor = colorIndex;
                                if (!Special(words[w][i], out fontColor)) fontColor = colorMap.GetPixel(lastColor % 6, lastColor / 6);
                                textTexture.SetPixel(paperIndex, 0, colorID);
                                textTexture.SetPixel(paperIndex, 1, fontColor);
                                time = (int)(Time.time * 10);
                            float    b = (time % 100) * 0.01f;
                                g = ((time / 100) % 100) * 0.01f;
                                r = (time / 10000) * 0.01f;
                                textTexture.SetPixel(paperIndex, 2, new Color(r, g, b));
                                 }
                            textIndex++;
                        }
                        else textTexture.SetPixel(paperIndex, 0, Color.black);
                        paperIndex++;
                        caret++;
                    }
            }
            while (paperIndex < maxCharacter)
                textTexture.SetPixel(paperIndex++, 0, Color.black);

            textTexture.Apply();
        }
    }
    [System.Serializable]
    public class SpecialColors
    {
        public string chars;
        public Color color;
    }
    public class TimedCharacter
    {
        public string letter;
        public float time;
        public Color color;
        public TimedCharacter(string l, float t, Color c)
        {
            letter = l;
            time = t;
            color = c;
        }
        public static List<TimedCharacter> Text(string s, float startTime, float appearWait, float spaceWait = 0, int charLength = 1)
        {
            List<TimedCharacter> list = new List<TimedCharacter>();
            float time = startTime;
            for (int i = 0; i < s.Length; i += charLength)
            {
                char ch = s[i * charLength];
                float wait = ch == ' ' ? spaceWait : appearWait;
                TimedCharacter tc = new TimedCharacter(s.Substring(i, charLength), time + wait, Color.white);
                time += wait;
                list.Add(tc);
            }
            return list;
        }
        public static void ChangeColor(List<TimedCharacter> list, Color c, string[] limitTo = null)
        {
            for (int i = 0; i < list.Count; i++)
                if (limitTo == null)
                    list[i].color = c;
                else
                    for (int j = 0; j < limitTo.Length; j++)
                        if (limitTo[j] == list[i].letter)
                        {
                            list[i].color = c;
                            break;
                        }
        }
    }
}