using UnityEngine;
using System.Collections.Generic;
using TMPro;


[CreateAssetMenu(fileName = "UITheme", menuName = "UI System/UI Theme")]
public class UITheme : ScriptableObject {
    public UIThemeType uiThemeType = UIThemeType.UI_THEME_LIGHT;
    public UIStyle uiStyle = UIStyle.UI_STYLE_AQUA_GREEN;
    public TMP_FontAsset font;

    // IMAGE COLOR
    // ----------------------------------------------------------------------------------------------
    public Color ColorBackground {get { return UIThemeUtil.ColorImageBackgroundFromUITheme(this); }}
    public Color ColorPrimary {get { return UIThemeUtil.ColorImagePrimaryFromUITheme(this); }}
    public Color ColorSecondary {get { return UIThemeUtil.ColorImageSecondaryFromUITheme(this); }}
    public Color ColorAccent {get { return UIThemeUtil.ColorImageAccentFromUITheme(this); }}
    public Color ColorNeutral {get { return UIThemeUtil.ColorImageNeutralFromUITheme(this); }}
    // TEXT COLOR
    // ----------------------------------------------------------------------------------------------
    public Color ColorTextPrimary {get { return UIThemeUtil.ColorTextPrimaryFromUITheme(this); }}
    public Color ColorTextSecondary {get { return UIThemeUtil.ColorTextSecondaryFromUITheme(this); }}
}

public enum UIThemeType {
    UI_THEME_LIGHT,
    UI_THEME_DARK,
}

public enum UIStyle {
    UI_STYLE_AQUA_GREEN,
}

public enum UIObjectImageColor {
    UI_OBJECT_IMAGE_COLOR_BACKGROUND,
    UI_OBJECT_IMAGE_COLOR_PRIMARY,
    UI_OBJECT_IMAGE_COLOR_SECONDARY,
    UI_OBJECT_IMAGE_COLOR_ACCENT,
    UI_OBJECT_IMAGE_COLOR_NEUTRAL,
}
public enum UIObjectTextColor {
    UI_OBJECT_TEXT_COLOR_PRIMARY,
    UI_OBJECT_TEXT_COLOR_SECONDARY
}

// Themes and styles from: https://colorsui.com/.
public static class UIThemeUtil {
    public static Dictionary<UIStyle,string> UIStyleToName
        = new Dictionary<UIStyle,string> {
            {UIStyle.UI_STYLE_AQUA_GREEN,"AquaGreen"}
        };

    public static Color ColorFromUIObjectImageColor(UIObjectImageColor uiObjectImageColor, UITheme uiTheme) {
        switch (uiObjectImageColor) {
            case UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_BACKGROUND:
                return uiTheme.ColorBackground;
            case UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_PRIMARY:
                return uiTheme.ColorPrimary;
            case UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_SECONDARY:
                return uiTheme.ColorSecondary;
            case UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_ACCENT:
                return uiTheme.ColorAccent;
            case UIObjectImageColor.UI_OBJECT_IMAGE_COLOR_NEUTRAL:
                return uiTheme.ColorNeutral;
            default:
                return uiTheme.ColorNeutral;
        }
    }
    public static Color ColorFromUIObjectTextColor(UIObjectTextColor uiObjectTextColor, UITheme uiTheme) {
        switch (uiObjectTextColor) {
            case UIObjectTextColor.UI_OBJECT_TEXT_COLOR_PRIMARY:
                return uiTheme.ColorTextPrimary;
            case UIObjectTextColor.UI_OBJECT_TEXT_COLOR_SECONDARY:
                return uiTheme.ColorTextSecondary;
            default:
                return uiTheme.ColorTextPrimary;
        }
    }

    // Colors are: [primary_dark,secondary_dark,accent,secondary_light,primary_light].
    public static Dictionary<UIStyle,List<Color>> UIStyleToColorList
        = new Dictionary<UIStyle,List<Color>> {
            // Palette: https://colorsui.com/view-palette/?palette=2a044a-0b2e59-0d6759-7ab317-a0c55f.
            {UIStyle.UI_STYLE_AQUA_GREEN,new List<Color> {new Color(24f/255f,4f/255f,74f/255f),
                                        new Color(11f/255f,46f/255f,89f/255f),
                                        new Color(13f/255f,229,255f,168/255f),
                                        new Color(122f/255f,179f/255f,23f/255f),
                                        new Color(160f/255f,197f/255f,95f/255f)}},
            
        };

    // IMAGE COLOR
    // ----------------------------------------------------------------------------------------------
    public static Color ColorImageBackgroundFromUITheme(UITheme uiTheme) {
        switch(uiTheme.uiThemeType) {
            case UIThemeType.UI_THEME_DARK: return Color.black;
            case UIThemeType.UI_THEME_LIGHT: return Color.white;
        }
        return Color.white;
    }
    public static Color ColorImagePrimaryFromUITheme(UITheme uiTheme) {
        // First color is associated with 'dark' tones which works with light text.
        // Last color is associated with 'light' tones which works with dark text.
        UIStyle uiStyle = UIStyle.UI_STYLE_AQUA_GREEN;
        switch(uiTheme.uiThemeType) {
            case UIThemeType.UI_THEME_DARK: return UIThemeUtil.UIStyleToColorList[uiStyle][0];
            case UIThemeType.UI_THEME_LIGHT: return UIThemeUtil.UIStyleToColorList[uiStyle][4];
        }
        return Color.green;
    }
    public static Color ColorImageSecondaryFromUITheme(UITheme uiTheme) {
        // First color is associated with 'dark' tones which works with light text.
        // Last color is associated with 'light' tones which works with dark text.
        UIStyle uiStyle = UIStyle.UI_STYLE_AQUA_GREEN;
        switch(uiTheme.uiThemeType) {
            case UIThemeType.UI_THEME_DARK: return UIThemeUtil.UIStyleToColorList[uiStyle][1];
            case UIThemeType.UI_THEME_LIGHT: return UIThemeUtil.UIStyleToColorList[uiStyle][3];
        }
        return Color.blue;
    }
    public static Color ColorImageAccentFromUITheme(UITheme uiTheme) {
        return UIThemeUtil.UIStyleToColorList[uiTheme.uiStyle][2];
    }
    public static Color ColorImageNeutralFromUITheme(UITheme uiTheme) {
        return new Color(211f/255f,211f/255f,211f/255f);
    }

    // TEXT COLOR
    // ----------------------------------------------------------------------------------------------
    public static Color ColorTextPrimaryFromUITheme(UITheme uiTheme) {
        switch(uiTheme.uiThemeType) {
            case UIThemeType.UI_THEME_DARK: return Color.white;
            case UIThemeType.UI_THEME_LIGHT: return Color.black;
        }
        return Color.black;
    }
    public static Color ColorTextSecondaryFromUITheme(UITheme uiTheme) {
        switch(uiTheme.uiThemeType) {
            case UIThemeType.UI_THEME_DARK: return new Color(224f/255f,224f/255f,224f/255f);
            case UIThemeType.UI_THEME_LIGHT: return new Color(117f/255f,117f/255f,117f/255f);
        }
        return new Color(224f/255f,224f/255f,224f/255f);
    }
}
