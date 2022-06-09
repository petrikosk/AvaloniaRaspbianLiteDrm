using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;

namespace AvaloniaRaspbianLiteDrm
{
    public class CustomFontManagerImpl : IFontManagerImpl
    {
        private readonly Typeface[] _customTypefaces;
        private readonly string _defaultFamilyName;

        private readonly Typeface _defaultTypeface =
            new Typeface("resm:AvaloniaApplication1.Assets.Fonts.msyh#Noto Mono");
        private readonly Typeface _italicTypeface =
            new Typeface("resm:AvaloniaApplication1.Assets.Fonts.msyh#Noto Sans", FontStyle.Italic);
        private readonly Typeface _emojiTypeface =
            new Typeface("resm:AvaloniaApplication1.Assets.Fonts.msyh#Twitter Color Emoji");
        private readonly Typeface _yaHeiTypeface =
            new Typeface("resm:AvaloniaApplication1.Assets.Fonts.msyh#Microsoft YaHei");

        private readonly Typeface _default = new Typeface("Liberation Mono");

        public CustomFontManagerImpl()
        {
            _customTypefaces = new[] { _emojiTypeface, _italicTypeface, _defaultTypeface };
            _defaultFamilyName = _defaultTypeface.FontFamily.FamilyNames.PrimaryFamilyName;
        }

        public string GetDefaultFontFamilyName()
        {
            return _defaultFamilyName;
        }

        public IEnumerable<string> GetInstalledFontFamilyNames(bool checkForUpdates = false)
        {
            return _customTypefaces.Select(x => x.FontFamily.Name);
        }

        private readonly string[] _bcp47 = { CultureInfo.CurrentCulture.ThreeLetterISOLanguageName, CultureInfo.CurrentCulture.TwoLetterISOLanguageName };

        public bool TryMatchCharacter(int codepoint, FontStyle fontStyle, FontWeight fontWeight,
            FontFamily fontFamily,
            CultureInfo culture, out Typeface typeface)
        {
            foreach (var customTypeface in _customTypefaces)
            {
                if (customTypeface.GlyphTypeface.GetGlyph((uint)codepoint) == 0)
                {
                    continue;
                }

                typeface = new Typeface(customTypeface.FontFamily, fontStyle, fontWeight);

                return true;
            }

            var fallback = SKFontManager.Default.MatchCharacter(fontFamily?.Name, (SKFontStyleWeight)fontWeight,
                SKFontStyleWidth.Normal, (SKFontStyleSlant)fontStyle, _bcp47, codepoint);

            typeface = new Typeface(fallback?.FamilyName ?? _defaultFamilyName, fontStyle, fontWeight);

            return true;
        }

        


        public IGlyphTypefaceImpl CreateGlyphTypeface(Typeface typeface)
        {
            SKTypeface skTypeface;

            switch (typeface.FontFamily.Name)
            {
                case "Twitter Color Emoji":
                    {
                        skTypeface = SKTypeface.FromFamilyName(_emojiTypeface.FontFamily.Name);
                        break;
                    }
                case "Noto Sans":
                    {
                        skTypeface = SKTypeface.FromFamilyName(_italicTypeface.FontFamily.Name);
                        break;
                    }
                case FontFamily.DefaultFontFamilyName:
                case "Noto Mono":
                    {
                        skTypeface = SKTypeface.FromFamilyName(_defaultTypeface.FontFamily.Name);
                        break;
                    }
                case "Microsoft YaHei":
                    {
                        skTypeface = SKTypeface.FromFamilyName(_yaHeiTypeface.FontFamily.Name);
                        break;
                    }
                default:
                    {
                        skTypeface = SKTypeface.FromFamilyName(typeface.FontFamily.Name,
                            (SKFontStyleWeight)typeface.Weight, SKFontStyleWidth.Normal, (SKFontStyleSlant)typeface.Style) ??
                                     SKTypeface.FromFamilyName(_default.FontFamily.Name);
                        break;
                    }
            }

            return new GlyphTypefaceImpl(skTypeface);
        }
    }


}