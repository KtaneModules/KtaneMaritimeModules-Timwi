namespace MaritimeFlags
{
    public sealed class Flag
    {
        public FlagDesign Design;
        public ColorInfo[] Colors;
        public bool Cutout;

        public Flag(FlagDesign design, ColorInfo[] colors, bool cutout = false)
        {
            Design = design;
            Colors = colors;
            Cutout = cutout;
        }
    }
}