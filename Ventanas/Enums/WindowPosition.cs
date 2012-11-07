using Base2io.Util.EnumUtil;

namespace Base2io.Ventanas.Enums
{
    public enum WindowPosition
    {
        [StringValue("Left 1/3")]
        LeftOneThird,

        [StringValue("Left half")]
        LeftHalf,

        [StringValue("Left 2/3")]
        LeftTwoThirds,

        [StringValue("Right 1/3")]
        RightOneThird,

        [StringValue("Right half")]
        RightHalf,

        [StringValue("Right 2/3")]
        RightTwoThirds,

        [StringValue("Top half")]
        TopHalf,

        [StringValue("Bottom half")]
        BottomHalf,

        [StringValue("Center")]
        Center,

        [StringValue("Fill")]
        Fill
    }
}
