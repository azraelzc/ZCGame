
namespace Battle {

public struct RectEdgeInt {

    public int leftEdge { get; set; }
    public int rightEdge { get; set; }
    public int topEdge { get; set; }
    public int downEdge { get; set; }

    public int GetHorizontalCenter() {
        return (leftEdge + rightEdge) / 2;
    }

    public int GetVerticalCenter() {
        return (topEdge + downEdge) / 2;
    }

    public VectorInt GetCenter() {
        return new VectorInt(GetHorizontalCenter(), GetVerticalCenter());
    }

    public VectorInt GetSize() {
        return new VectorInt(System.Math.Abs(rightEdge - leftEdge), System.Math.Abs(topEdge - downEdge));
    }

    public bool IsInside(VectorInt position) {
        return position.x > leftEdge && position.x < rightEdge && position.y < topEdge && position.y > downEdge;
    }

    public RectEdge ToRectEdge() {
        return new RectEdge(leftEdge, rightEdge, topEdge, downEdge);
    }

    public static implicit operator RectEdge(RectEdgeInt self) {
        return self.ToRectEdge();
    }

    public override string ToString() {
        return string.Format("left:{0} right:{1} top:{2} down:{3}", leftEdge, rightEdge, topEdge, downEdge);
    }

    //public static bool operator ==(RectEdge x, RectEdge y) {
    //    return x.leftEdge == y.leftEdge && x.rightEdge == y.rightEdge && x.topEdge == y.topEdge && x.downEdge == y.downEdge;
    //}
    //public static bool operator !=(RectEdge x, RectEdge y) {
    //    return x.leftEdge != y.leftEdge || x.rightEdge != y.rightEdge || x.topEdge != y.topEdge || x.downEdge != y.downEdge;
    //}

}
}