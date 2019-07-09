
namespace Battle {

public struct RectEdge {
    public Number left { get; set; }
    public Number right { get; set; }
    public Number top { get; set; }
    public Number down { get; set; }

    public RectEdge(Number left, Number right, Number top, Number down) {
        this.left = left;
        this.right = right;
        this.top = top;
        this.down = down;
    }

    public Vector size { get { return new Vector(Math.Abs(right - left), Math.Abs(top - down)); } }

    public bool IsInside(Vector position) {
        return position.x > left && position.x < right && position.y < top && position.y > down;
    }

    public Vector GetCenter() {
        return new Vector(left + right, top + down) / 2;
    }

    public static RectEdge operator * (RectEdge self, Number v) {
        self.left *= v;
        self.right *= v;
        self.top *= v;
        self.down *= v;
        return self;
    }

    public static RectEdge operator + (RectEdge self, RectEdge v) {
        self.left += v.left;
        self.right += v.right;
        self.top += v.top;
        self.down += v.down;
        return self;
    }

    public static RectEdge operator - (RectEdge self, RectEdge v) {
        self.left -= v.left;
        self.right -= v.right;
        self.top -= v.top;
        self.down -= v.down;
        return self;
    }

    public override string ToString() {
        return string.Format("left:{0} right:{1} top:{2} down:{3}", left, right, top, down);
    }
}

}
