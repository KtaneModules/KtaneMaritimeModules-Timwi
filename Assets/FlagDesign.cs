using System;

namespace MaritimeFlags
{
    public sealed class FlagDesign
    {
        public string NameFmt;
        public int NumColors;
        public bool ReverseAllowed;
        public bool CutoutAllowed;
        public bool IsRepeater;
        public Func<double, double, int> GetPixel;
    }
}