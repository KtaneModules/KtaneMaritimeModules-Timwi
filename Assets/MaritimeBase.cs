using System;
using System.Collections.Generic;
using System.Linq;
using MaritimeFlags;
using UnityEngine;

public abstract class MaritimeBase : MonoBehaviour
{
    protected abstract string Name { get; }

    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;
    public KMRuleSeedable RuleSeedable;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    protected bool _isSolved;

    protected static T[] newArray<T>(params T[] array)
    {
        return array;
    }

    protected static T[] newArray<T>(int length, Func<int, T> fnc)
    {
        var arr = new T[length];
        for (int i = 0; i < length; i++)
            arr[i] = fnc(i);
        return arr;
    }

    private static readonly int[] _plussesDesign = new[] { 17, 27, 31, 32, 33, 41, 42, 43, 47, 57, 97, 111, 112, 113, 127, 167, 177, 181, 182, 183, 191, 192, 193, 197, 207 };
    private static readonly FlagDesign[] _flagDesigns = newArray(
        new FlagDesign { NameFmt = @"plain {0}", NumColors = 1, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => 0 },
        new FlagDesign { NameFmt = @"{0}-{1} vertical", NumColors = 2, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => x >= .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} vertical", NumColors = 3, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) % 3 },
        new FlagDesign { NameFmt = @"{1}-{0}-{1} vertical", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => ((int) (x * 3) % 2) ^ 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} vertical uneven", NumColors = 2, ReverseAllowed = true, CutoutAllowed = false, GetPixel = (x, y) => x >= .8 || x < .2 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} 6 vertical stripes", NumColors = 2, ReverseAllowed = false, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 6) % 2 },
        new FlagDesign { NameFmt = @"{0}-{1} horizontal", NumColors = 2, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} horizontal", NumColors = 3, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => 2 - (int) (y * 3) % 3 },
        new FlagDesign { NameFmt = @"{1}-{0}-{1} horizontal", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => ((int) (y * 3) % 2) ^ 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} horizontal uneven", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => y >= .8 || y < .2 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2}-{1}-{0} horizontal", NumColors = 3, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => y >= .8 ? 0 : y >= .6 ? 1 : y >= .4 ? 2 : y >= .2 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} 6 horizontal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => 1 - (int) (y * 6) % 2 },
        new FlagDesign { NameFmt = @"{1} diamond on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Abs(x - y) < .5 && Math.Abs(x + y - 1) < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} centered circle on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{2} circle on {1} circle on {0}", ReverseAllowed = true, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => { var d = Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2); return d < .04 ? 2 : d < .16 ? 1 : 0; } },
        new FlagDesign { NameFmt = @"{1}-{0} 2×2 checkerboard", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (x >= .5) ^ (y >= .5) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1}-{2} orthogonal quadrants on {0}", ReverseAllowed = false, NumColors = 3, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? 0 : x >= .5 ? 2 : 1 },
        new FlagDesign { NameFmt = @"{0}-{2}-{1}-{3} orthogonal quadrants", ReverseAllowed = false, NumColors = 4, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? (x >= .5 ? 1 : 3) : (x >= .5 ? 2 : 0) },
        new FlagDesign { NameFmt = @"{1}-{0} 4×4 checkerboard", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => ((int) (x * 4) % 2 == 0) ^ ((int) (y * 4) % 2 == 0) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} saltire on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Abs(x - y) < .125 || Math.Abs(x + y - 1) < .125 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} diagonal", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => y > 1 - x ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} centered square on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{2} square on {1} square on {0}", ReverseAllowed = true, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => { var x1 = (int) (x * 5); var y1 = (int) (y * 5); return x1 == 2 && y1 == 2 ? 2 : x1 >= 1 && x1 <= 3 && y1 >= 1 && y1 <= 3 ? 1 : 0; } },
        new FlagDesign { NameFmt = @"{2} square on {0}-{1} diagonal", ReverseAllowed = false, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 2 : (1 - y) > x ? 0 : 1 },
        new FlagDesign { NameFmt = @"{2} square on {0}-{1} horizontal", ReverseAllowed = false, NumColors = 3, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 2 : y < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} cross on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 5) == 2 || (int) (y * 5) == 2 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} 7 diagonal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (3.5 * (x - y + 1)) % 2 == 0 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} 10 diagonal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (7 * (x - y + 2)) % 2 == 0 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{3}-{1}-{2} diagonal quadrants", ReverseAllowed = false, NumColors = 4, CutoutAllowed = false, GetPixel = (x, y) => y > x ? (y > 1 - x ? 0 : 2) : (y > 1 - x ? 3 : 1) },
        new FlagDesign { NameFmt = @"{0}-{1} horizontal semicircles pattern", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625) ^ (y >= .5) ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} diagonal semicircles pattern", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625) ^ (1 - y < x) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} triangle on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => y > x * 3 / 7.5 + .2 && y < -x * 3 / 7.5 + .8 ? 1 : 0 },
        new FlagDesign { NameFmt = @"2 {1} triangles on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (y > x / 4 && y < -x / 4 + .5) || (y > x / 4 + .5 && y < 1 - x / 4) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} plusses on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => _plussesDesign.Contains((int) (x * 15) + 15 * ((int) (y * 15))) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} square at edge on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => y >= .3 && y < .7 && x < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} triangle regions", ReverseAllowed = false, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => x * 2 > y + 1 ? 2 : x / 2 > y - .5 ? 1 : 0 });
    private static readonly FlagDesign[] _repeaterDesigns = newArray(
        new FlagDesign { NameFmt = @"{1} triangle on {0}", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => 3.2 * (y - .1) > x && -3.2 * (y - .525) > x ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} vertical", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => x >= .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} horizontal", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => y >= .2 && y < .425 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} square on {0}", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => y >= .2 && y < .425 && x < .225 ? 1 : 0 });

    private static readonly ColorInfo[][] _colorGroups = newArray(
        new[] { new ColorInfo { Name = "red", Color = Color.red } },
        new[] { new ColorInfo { Name = "blue", Color = Color.blue }, new ColorInfo { Name = "black", Color = Color.black } },
        new[] { new ColorInfo { Name = "yellow", Color = new Color(1, 1, 0) }, new ColorInfo { Name = "white", Color = Color.white } });

    protected abstract void DoStart(MonoRandom rnd);

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        _isSolved = false;

        var rnd = RuleSeedable.GetRNG();
        Log("Using rule seed: {0}", rnd.Seed);
        DoStart(rnd);
    }

    protected Flag[] GenerateFlags(MonoRandom rnd)
    {
        var red = _colorGroups[0][0];
        var blue = _colorGroups[1][0];
        var black = _colorGroups[1][1];
        var yellow = _colorGroups[2][0];
        var white = _colorGroups[2][1];

        if (rnd.Seed == 1)
        {
            return new Flag[]
            {
                new Flag(_flagDesigns[1], new[] { white, blue }, cutout: true),
                new Flag(_flagDesigns[0], new[] { red }, cutout: true),
                new Flag(_flagDesigns[10], new[] { blue, white, red }),
                new Flag(_flagDesigns[9], new[] { yellow, blue }),
                new Flag(_flagDesigns[6], new[] { blue, red }),
                new Flag(_flagDesigns[12], new[] { white, red }),
                new Flag(_flagDesigns[5], new[] { yellow, blue }),
                new Flag(_flagDesigns[1], new[] { white, red }),
                new Flag(_flagDesigns[13], new[] { yellow, black }),
                new Flag(_flagDesigns[8], new[] { white, blue }),
                new Flag(_flagDesigns[1], new[] { yellow, blue }),
                new Flag(_flagDesigns[15], new[] { black, yellow }),
                new Flag(_flagDesigns[19], new[] { blue, white }),
                new Flag(_flagDesigns[18], new[] { white, blue }),
                new Flag(_flagDesigns[20], new[] { yellow, red }),
                new Flag(_flagDesigns[21], new[] { blue, white }),
                new Flag(_flagDesigns[0], new[] { yellow }),
                new Flag(_flagDesigns[25], new[] { red, yellow }),
                new Flag(_flagDesigns[21], new[] { white, blue }),
                new Flag(_flagDesigns[2], new[] { red, white, blue }),
                new Flag(_flagDesigns[15], new[] { white, red }),
                new Flag(_flagDesigns[19], new[] { white, red }),
                new Flag(_flagDesigns[22], new[] { blue, white, red }),
                new Flag(_flagDesigns[25], new[] { white, blue }),
                new Flag(_flagDesigns[27], new[] { yellow, red }),
                new Flag(_flagDesigns[28], new[] { yellow, red, black, blue }),
                new Flag(_flagDesigns[33], new[] { white, blue }),
                new Flag(_flagDesigns[8], new[] { yellow, red }),
                new Flag(_flagDesigns[8], new[] { red, yellow }),
                new Flag(_flagDesigns[8], new[] { red, blue }),
                new Flag(_flagDesigns[19], new[] { red, white }),
                new Flag(_flagDesigns[19], new[] { yellow, blue }),
                new Flag(_flagDesigns[26], new[] { white, blue }),
                new Flag(_flagDesigns[3], new[] { white, red }),
                new Flag(_flagDesigns[3], new[] { blue, yellow }),
                new Flag(_flagDesigns[3], new[] { white, blue }),
                new Flag(_repeaterDesigns[0], new[] { blue, yellow }),
                new Flag(_repeaterDesigns[1], new[] { blue, white }),
                new Flag(_repeaterDesigns[2], new[] { white, black }),
                new Flag(_repeaterDesigns[3], new[] { red, yellow })
            };
        }
        else if (rnd.Seed == 0)
        {
            var generatedFlags = new Flag[40];
            for (var k = 0; k < 2; k++)
            {
                var designs = k == 0 ? _flagDesigns : _repeaterDesigns;
                for (var i = 0; i < designs.Length; i++)
                {
                    var colors = new ColorInfo[designs[i].NumColors];
                    for (var j = 0; j < designs[i].NumColors; j++)
                    {
                        var n = (float) (designs[i].NumColors == 1 ? .5 : .8 * j / (designs[i].NumColors - 1) + .1);
                        colors[j] = new ColorInfo { Color = new Color(n, n, n), Name = "Gray " + n };
                    }
                    generatedFlags[(k == 1) ? 36 + i : i] = new Flag(designs[i], colors, designs[i].CutoutAllowed);
                }
            }
            return generatedFlags;
        }
        else
        {
            var letterNumberFlags = generateFlags(36, _flagDesigns, rnd, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Select(ch => ch.ToString()).ToArray());
            var repeaterFlags = generateFlags(4, _repeaterDesigns, rnd, Enumerable.Range(1, 4).Select(i => "Repeat " + i).ToArray());
            return letterNumberFlags.Concat(repeaterFlags).ToArray();
        }
    }

    protected void DebugLog(string format, params object[] args) { Log(format, true, args); }
    protected void Log(string format, params object[] args) { Log(format, false, args); }
    private void Log(string format, bool isDebug, object[] args)
    {
        Debug.LogFormat(@"{0}{1} #{2}{3} {4}",
            isDebug ? "<" : "[",
            Name,
            _moduleId,
            isDebug ? ">" : "]",
            string.Format(format, args));
    }

    private const int _flagWidth = 519;
    private const int _flagHeight = 519;
    private const int _flagPadding = 16;
    private const int _flagThickness = 8;

    protected Texture2D GenerateFlagTexture(Flag flag)
    {
        var tx = new Texture2D(_flagWidth, _flagHeight, TextureFormat.ARGB32, false);
        tx.SetPixels(newArray(_flagWidth * _flagHeight, i =>
        {
            var x = i % _flagWidth;
            var y = i / _flagWidth;

            if (flag.Design.IsRepeater)
            {
                const int pp = (_flagHeight - (_flagHeight - 2 * _flagPadding) * 5 / 8) / 2;
                // Left frame
                if (x > _flagPadding - _flagThickness / 2 && x < _flagPadding + _flagThickness / 2 && y > pp && y < _flagHeight - pp)
                    return Color.black;
                // Top frame
                else if (x > _flagPadding - _flagThickness / 2 && y <= _flagWidth / 2 && distancePointToLine(x, y, _flagPadding, pp, _flagWidth - _flagPadding, _flagHeight / 2) < _flagThickness / 2)
                    return Color.black;
                // Bottom frame
                else if (x > _flagPadding - _flagThickness / 2 && y >= _flagWidth / 2 && distancePointToLine(x, y, _flagPadding, _flagHeight - pp, _flagWidth - _flagPadding, _flagHeight / 2) < _flagThickness / 2)
                    return Color.black;
                // Flag body
                else if (x > _flagPadding && 3.2 * (y - pp) > x - _flagPadding && -3.2 * (y - _flagHeight + pp) > x - _flagPadding)
                    return flag.Colors[flag.Design.GetPixel((x - _flagPadding) / (double) (_flagWidth - 2 * _flagPadding), (y - pp) / (double) (_flagHeight - 2 * _flagPadding))].Color;
            }
            else if (flag.Cutout)
            {
                const int iw = _flagWidth - 2 * _flagPadding;
                // Top frame
                if (x > _flagPadding - _flagThickness / 2 && x < _flagWidth - _flagPadding && y > _flagPadding - _flagThickness / 2 && y < _flagPadding + _flagThickness / 2)
                    return Color.black;
                // Bottom frame
                else if (x > _flagPadding - _flagThickness / 2 && x < _flagWidth - _flagPadding && y > _flagHeight - _flagPadding - _flagThickness / 2 && y < _flagHeight - _flagPadding + _flagThickness / 2)
                    return Color.black;
                // Left frame
                else if (x > _flagPadding - _flagThickness / 2 && x < _flagPadding + _flagThickness / 2 && y > _flagPadding - _flagThickness / 2 && y < _flagHeight - _flagPadding + _flagThickness / 2)
                    return Color.black;
                // Bottom half of right frame
                else if (y > _flagPadding - _flagThickness / 2 && y <= _flagHeight / 2 && Math.Abs(x - _flagPadding - iw * 3 / 4 - (-y + _flagWidth / 2) / 2) < _flagThickness / 2)
                    return Color.black;
                // Top half of right frame
                else if (y >= _flagHeight / 2 && y < _flagHeight - _flagPadding + _flagThickness / 2 && Math.Abs(x - _flagPadding - iw * 3 / 4 - (y - _flagWidth / 2) / 2) < _flagThickness / 2)
                    return Color.black;
                // Flag body
                else if (x > _flagPadding && y > _flagPadding && y < _flagHeight - _flagPadding && ((x - _flagPadding - iw * 3 / 4) < (y - _flagWidth / 2) / 2 || (x - _flagPadding - iw * 3 / 4) < (-y + _flagWidth / 2) / 2))
                    return flag.Colors[flag.Design.GetPixel((x - _flagPadding) / (double) (_flagWidth - 2 * _flagPadding), (y - _flagPadding) / (double) (_flagHeight - 2 * _flagPadding))].Color;
            }
            else
            {
                // Border
                if (x > _flagPadding - _flagThickness / 2 && x < _flagWidth - _flagPadding + _flagThickness / 2 && y > _flagPadding - _flagThickness / 2 && y < _flagHeight - _flagPadding + _flagThickness / 2 &&
                    !(x > _flagPadding + _flagThickness / 2 && x < _flagWidth - _flagPadding - _flagThickness / 2 && y > _flagPadding + _flagThickness / 2 && y < _flagHeight - _flagPadding - _flagThickness / 2))
                    return Color.black;
                // Flag body
                else if (x > _flagPadding && x < _flagWidth - _flagPadding && y > _flagPadding && y < _flagHeight - _flagPadding)
                    return flag.Colors[flag.Design.GetPixel((x - _flagPadding) / (double) (_flagWidth - 2 * _flagPadding), (y - _flagPadding) / (double) (_flagHeight - 2 * _flagPadding))].Color;
            }
            return new Color(0, 0, 0, 0);
        }));
        tx.Apply();
        tx.wrapMode = TextureWrapMode.Clamp;
        return tx;
    }

    protected Sprite GenerateFlagSprite(Flag flag)
    {
        return Sprite.Create(GenerateFlagTexture(flag), new Rect(0, 0, _flagWidth, _flagHeight), new Vector2(.5f, .5f));
    }

    private List<Flag> generateFlags(int count, FlagDesign[] designs, MonoRandom rnd, string[] flagNames)
    {
        // Each design can be used different numbers of times
        var designIxsAvailable = new List<int>();
        for (var i = 0; i < designs.Length; i++)
        {
            var allowed =
                designs.Length == 4 ? 1 :
                designs[i].NumColors == 4 ? 1 :
                designs[i].NumColors == 2 && designs[i].ReverseAllowed ? 4 : 3;
            for (var j = 0; j < allowed; j++)
                designIxsAvailable.Add(i);
        }

        // Assign designs at random
        var flags = new List<Flag>();
        var colorCombinations = new Dictionary<int, List<int[]>>();
        var availableColorIxs = Enumerable.Range(0, _colorGroups.Length).ToList();
        for (var i = 0; i < count; i++)
        {
            var ix = rnd.Next(0, designIxsAvailable.Count);
            var designIx = designIxsAvailable[ix];
            designIxsAvailable.RemoveAt(ix);

            ColorInfo[] flagColors;

            if (designs[designIx].NumColors == 4)
            {
                // 4-color designs are allowed to have blue+black and/or yellow+white, but only in a specific order.
                flagColors = new ColorInfo[4];
                var fcIx = 0;
                rnd.ShuffleFisherYates(availableColorIxs);
                var ixIx = 0;
                while (fcIx < 4)
                {
                    for (var j = 0; j < _colorGroups[availableColorIxs[ixIx]].Length && fcIx < 4; j++)
                    {
                        flagColors[fcIx] = _colorGroups[availableColorIxs[ixIx]][j];
                        fcIx++;
                        if (fcIx % 2 == 0)
                            break;
                    }
                    ixIx++;
                }
            }
            else
            {
                // Find a random color combination that hasn’t been used yet.
                // Use only one color per group so we don’t get black+blue or yellow+white on the same flag.
                int[] colorIxs;
                do
                {
                    rnd.ShuffleFisherYates(availableColorIxs);
                    colorIxs = availableColorIxs.Take(designs[designIx].NumColors).ToArray();
                }
                while (colorCombinations.ContainsKey(designIx) && colorCombinations[designIx].Any(cc => cc.SequenceEqual(colorIxs) || (!designs[designIx].ReverseAllowed && cc.Reverse().SequenceEqual(colorIxs))));
                if (!colorCombinations.ContainsKey(designIx))
                    colorCombinations[designIx] = new List<int[]>();
                colorCombinations[designIx].Add(colorIxs);

                do
                    flagColors = colorIxs.Select(cix => _colorGroups[cix][rnd.Next(0, _colorGroups[cix].Length)]).ToArray();
                // Special case: don’t allow entirely black-and-white flags
                while (designs[designIx].NumColors == 2 && ((flagColors[0].Name == "black" && flagColors[1].Name == "white") || (flagColors[0].Name == "white" && flagColors[1].Name == "black")));
            }
            var cutout = designs[designIx].CutoutAllowed && (rnd.Next(0, 10) == 0);
            DebugLog("Flag {0} is {1}{2}", flagNames[i], string.Format(designs[designIx].NameFmt, flagColors.Select(cc => (object) cc.Name).ToArray()), cutout ? " with cutout" : "");
            flags.Add(new Flag(designs[designIx], flagColors, cutout: cutout));
        }
        return flags;
    }

    double distancePointToLine(double px, double py, double lx1, double ly1, double lx2, double ly2)
    {
        var dirX = lx2 - lx1;
        var dirY = ly2 - ly1;
        var lambda = (dirX * (px - lx1) + dirY * (py - ly1)) / (dirX * dirX + dirY * dirY);
        return Math.Sqrt(Math.Pow(px - (lx1 + lambda * dirX), 2) + Math.Pow(py - (ly1 + lambda * dirY), 2));
    }
}
