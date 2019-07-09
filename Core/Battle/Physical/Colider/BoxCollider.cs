
using System.Collections.Generic;

namespace Battle.Core {

public class BoxCollider : Collider {

    public static readonly int FULL  = -1;
    public static readonly int LEFT  = 1 << 0;
    public static readonly int RIGHT = 1 << 1;
    public static readonly int TOP   = 1 << 2;
    public static readonly int DOWN  = 1 << 3;

    public int side { get; private set; }

    Vector m_cachedOffset;
    Vector m_cachedSize;
    Number m_cachedRotation;
    Vector[] m_cachedCorners;
    Vector[] m_cachedUnRotateCorners;

    public BoxCollider(Number x, Number y) : this(new Vector(x, y)) {
    }

    public BoxCollider(Number x, Number y, int side) : this(new Vector(x, y), side) {

    }

    public BoxCollider(Vector sz) : this(sz, FULL) {
    }

    public BoxCollider(Vector size, int side) : base() {
        this.size = size;
        this.side = side;
    }

    public void SetSize(Vector size) {
        this.size = size;
    }

    public override bool IsConcrete() {
        return (this.side & 0xF) == 0xF;
    }

    public override bool OverlapBox(Vector origin, Vector size, Number rotation) {
        if (this.rotation == Number.Zero && rotation == Number.Zero) {
            return Number.Abs(position.x-origin.x) <= (this.size.x+size.x) &&
                   Number.Abs(position.y-origin.y) <= (this.size.y+size.y);
        }
        var rect1 = PhysicsUtils.GetRectangleVertices(position, this.size, this.rotation);
        var rect2 = PhysicsUtils.GetRectangleVertices(origin, size, rotation);
        return PhysicsUtils.IsRectangleOverlap(rect1, rect2);
    }

    public override bool OverlapCircle(Vector origin, Number radius) {
        return PhysicsUtils.IsRectangleCircleOverlap(position, size, rotation, origin, radius);
    }

    public int IntersectSegment(Vector start, Vector end, List<Vector> results) {
        var n = 0;
        var corners = GetLocalCorners();
        for (int i = 0; i < corners.Length; i++) {
            Vector point = Vector.zero;
            if (PhysicsUtils.GetSegmentIntersection(
                start,
                end,
                corners[i] + position,
                corners[(i+1)%corners.Length] + position,
                ref point))
            {
                results.Add(point);
                n++;
            }
        }
        return n;
    }

    public bool IntersectSegmentNearest(Vector start, Vector end, out Vector result) {
        result = Vector.zero;
        var results = new List<Vector>();
        var n = IntersectSegment(start, end, results);
        if (n == 0) {
            return false;
        }

        result = results[0];
        var minDistance = Vector.Distance(result, start);
        for (int i = 1; i < results.Count; i++) {
            var distance = Vector.Distance(results[i], start);
            if (distance < minDistance) {
                result = results[i];
                minDistance = distance;
            }
        }

        return true;
    }

    public Vector[] GetLocalCorners() {
        if (m_cachedCorners == null ||
            m_cachedOffset != offset ||
            m_cachedSize != size ||
            m_cachedRotation != rotation)
        {
            if (m_cachedCorners == null) {
                m_cachedCorners = new Vector[4];
            }
            m_cachedOffset = offset;
            m_cachedSize = size;
            m_cachedRotation = rotation;
            GetUnRotateLocalCorners(m_cachedCorners);
            if (m_cachedRotation != Number.Zero) {
                for (int i = 0; i < m_cachedCorners.Length; i++) {
                    m_cachedCorners[i] = Vector.Rotate(Vector.zero, m_cachedCorners[i], m_cachedRotation);
                }
            }
        }
        return m_cachedCorners;
    }

    private Vector[] GetUnRotateLocalCorners() {
        if (m_cachedUnRotateCorners == null) {
            m_cachedUnRotateCorners = new Vector[4];
        }
        GetUnRotateLocalCorners(m_cachedUnRotateCorners);
        return m_cachedUnRotateCorners;
    }

    private void GetUnRotateLocalCorners(Vector[] corners) {
        corners[0].x = -size.x; corners[0].y = size.y;  // top-left
        corners[1].x = size.x; corners[1].y = size.y;   // top-right
        corners[2].x = size.x; corners[2].y = -size.y;  // bottom-right
        corners[3].x = -size.x; corners[3].y = -size.y; // bottom-left
        for (int i = 0; i < 4; i++) {
            corners[i] += offset;
        }
    }

}

}