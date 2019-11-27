using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


class ReplaceTextForTextMeshPro : EditorWindow
{
    [MenuItem("Antura/TMPro Utils/Select Text Components")]
    public static void SelectText(MenuCommand command)
    {
        Transform[] ts = FindObjectsOfType<Transform>();
        List<GameObject> selection = new List<GameObject>();
        foreach (Transform t in ts) {
            Text[] cs = t.gameObject.GetComponents<Text>();
            if (cs.Length > 0) {
                selection.Add(t.gameObject);
            }
        }
        Selection.objects = selection.ToArray();
    }

    [MenuItem("Antura/TMPro Utils/Convert selected Text -> TextMeshUGUI ")]
    public static void TextMeshTransmorphSingle(MenuCommand command)
    {
        Text currentUIText = Selection.activeGameObject.GetComponent<Text>();

        TMP_FontAsset newFont = Resources.Load("Fonts/AmaticSC-Bold SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
        if (newFont == null) {
            Debug.Log("Not so awesome...");
            return;
        }
        Dictionary<string, TMP_FontAsset> fontMap = new Dictionary<string, TMP_FontAsset>();
        fontMap.Add("Bangers", newFont);


        Font font = currentUIText.font;
        if (font.fontNames[0] != "Bangers")
            return;

        GameObject gameObject = currentUIText.gameObject;
        Selectable[] selectables = FindObjectsOfType<Selectable>();

        List<Selectable> targetGraphicRefs = new List<Selectable>();
        foreach (Selectable selectable in selectables) {
            if (selectable.targetGraphic == currentUIText) {
                targetGraphicRefs.Add(selectable);
            }
        }
        int fontSize = currentUIText.fontSize;
        FontStyle fontStyle = currentUIText.fontStyle;
        HorizontalWrapMode horizWrap = currentUIText.horizontalOverflow;
        string textValue = currentUIText.text;
        TextAnchor anchor = currentUIText.alignment;
        Color color = currentUIText.color;
        float lineSpacing = currentUIText.lineSpacing;

        DestroyImmediate(currentUIText);

        TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
        if (fontMap.ContainsKey(font.fontNames[0])) {
            textMesh.font = fontMap[font.fontNames[0]];
        }
        textMesh.fontSize = fontSize;
        textMesh.fontStyle = FontStyle2TMProFontStyle(fontStyle);
        textMesh.enableWordWrapping = (horizWrap == HorizontalWrapMode.Wrap);
        textMesh.text = textValue;
        textMesh.alignment = TextAnchor2TMProTextAlignmentOptions(anchor);
        textMesh.color = color;
        textMesh.lineSpacing = lineSpacing;

        foreach (Selectable selectable in targetGraphicRefs) {
            selectable.targetGraphic = textMesh;
        }
    }

    [MenuItem("Antura/TMPro Utils/Convert all Texts -> TextMeshUGUI ")]
    public static void TextMeshTransmorphAll(MenuCommand command)
    {
        Text[] texts = FindObjectsOfType<Text>();

        TMP_FontAsset newFont = Resources.Load("Fonts/Arial Latin SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
        if (newFont == null) {
            Debug.Log("Not so awesome...");
            return;
        }
        Dictionary<string, TMP_FontAsset> fontMap = new Dictionary<string, TMP_FontAsset>();
        fontMap.Add("Arial", newFont);

        foreach (Text text in texts) {
            Font font = text.font;
            if (font.fontNames[0] != "Arial")
                continue;

            GameObject gameObject = text.gameObject;
            Selectable[] selectables = FindObjectsOfType<Selectable>();

            List<Selectable> targetGraphicRefs = new List<Selectable>();
            foreach (Selectable selectable in selectables) {
                if (selectable.targetGraphic == text) {
                    targetGraphicRefs.Add(selectable);
                }
            }
            int fontSize = text.fontSize;
            FontStyle fontStyle = text.fontStyle;
            HorizontalWrapMode horizWrap = text.horizontalOverflow;
            string textValue = text.text;
            TextAnchor anchor = text.alignment;
            Color color = text.color;
            float lineSpacing = text.lineSpacing;

            DestroyImmediate(text);

            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            if (fontMap.ContainsKey(font.fontNames[0])) {
                textMesh.font = fontMap[font.fontNames[0]];
            }
            textMesh.fontSize = fontSize;
            textMesh.fontStyle = FontStyle2TMProFontStyle(fontStyle);
            textMesh.enableWordWrapping = (horizWrap == HorizontalWrapMode.Wrap);
            textMesh.text = textValue;
            textMesh.alignment = TextAnchor2TMProTextAlignmentOptions(anchor);
            textMesh.color = color;
            textMesh.lineSpacing = lineSpacing;

            foreach (Selectable selectable in targetGraphicRefs) {
                selectable.targetGraphic = textMesh;
            }
        }
    }

    private static TMPro.FontStyles FontStyle2TMProFontStyle(FontStyle style)
    {
        switch (style) {
            case FontStyle.Bold:
                return TMPro.FontStyles.Bold;
            case FontStyle.Normal:
                return TMPro.FontStyles.Normal;
            case FontStyle.Italic:
                return TMPro.FontStyles.Italic;
            case FontStyle.BoldAndItalic:
                return TMPro.FontStyles.Italic; // No choice for Bold & Italic
            default:
                return TMPro.FontStyles.Normal;
        }
    }

    private static TMPro.TextAlignmentOptions TextAnchor2TMProTextAlignmentOptions(TextAnchor anchor)
    {
        switch (anchor) {
            case TextAnchor.LowerCenter:
                return TMPro.TextAlignmentOptions.Bottom;
            case TextAnchor.LowerLeft:
                return TMPro.TextAlignmentOptions.BottomLeft;
            case TextAnchor.LowerRight:
                return TMPro.TextAlignmentOptions.BottomRight;
            case TextAnchor.MiddleCenter:
                return TMPro.TextAlignmentOptions.Center;
            case TextAnchor.MiddleLeft:
                return TMPro.TextAlignmentOptions.MidlineLeft;
            case TextAnchor.MiddleRight:
                return TMPro.TextAlignmentOptions.MidlineRight;
            case TextAnchor.UpperLeft:
                return TMPro.TextAlignmentOptions.TopLeft;
            case TextAnchor.UpperCenter:
                return TMPro.TextAlignmentOptions.Top;
            case TextAnchor.UpperRight:
                return TMPro.TextAlignmentOptions.TopRight;
            default:
                return TMPro.TextAlignmentOptions.Left;
        }
    }

    [MenuItem("Antura/TMPro Utils/Log Text Properties")]
    public static void LogTextFontProperties(MenuCommand command)
    {
        TMP_FontAsset newFont = Resources.Load("Fonts/Arial Latin SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
        if (newFont == null)
            Debug.Log("Not so awesome...");

        var selected = Selection.activeObject;
        if (selected && selected is GameObject) {
            Text text = ((GameObject) selected).GetComponent<Text>();
            if (text != null) {
                Debug.Log("Font is = " + text.font.fontNames[0] + "\n" +
                          "Style is = " + text.fontStyle
                );
                Selectable[] selectables = FindObjectsOfType<Selectable>();
                Debug.Log("Number of Selectables found: " + selectables.Length);

                foreach (Selectable selectable in Selectable.allSelectables) {
                    if (selectable.targetGraphic == text) {
                        Debug.Log("Selectable targetGraphic found: " + selectable.name + " (" + selectable.GetType() + ")");
                    }
                }
            }
        }
    }

    void OnGUI()
    {
    }
}