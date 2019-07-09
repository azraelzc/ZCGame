
namespace Battle.Core {

public class PhysicsUtils {

    public static readonly Number LENGTH_RAY_DISTANCE = 2000;

    public static bool GetSegmentIntersectionAligned(Vector h1, Vector h2, Vector v1, Vector v2, ref Vector point) {
        Vector vss, vse, hss, hse;

        if (v1.y < v2.y) {
            vss = v1; vse = v2;
        } else {
            vss = v2; vse = v1;
        }
        if (h1.x < h2.x) {
            hss = h1; hse = h2;
        } else {
            hss = h2; hse = h1;
        }

        if (hss.x <= vss.x && hse.x >= vss.x && vss.y <= hss.y && vse.y >= hss.y) {
            point.x = vss.x;
            point.y = hss.y;
            return true;
        }

        return false;
    }

    public static bool GetSegmentIntersection(Vector p0, Vector p1, Vector p2, Vector p3, ref Vector point) {
        var s10_x = p1.x - p0.x;
        var s10_y = p1.y - p0.y;
        var s32_x = p3.x - p2.x;
        var s32_y = p3.y - p2.y;

        var denom = s10_x * s32_y - s32_x * s10_y;

        if (denom == 0) return false; // no collision

        var denom_is_positive = denom > 0;

        var s02_x = p0.x - p2.x;
        var s02_y = p0.y - p2.y;

        var s_numer = s10_x * s02_y - s10_y * s02_x;
    
        if ((s_numer < 0) == denom_is_positive) return false; // no collision
    
        var t_numer = s32_x * s02_y - s32_y * s02_x;
    
        if ((t_numer < 0) == denom_is_positive) return false; // no collision
    
        if ((s_numer > denom) == denom_is_positive || (t_numer > denom) == denom_is_positive) return false; // no collision
    
        // collision detected
    
        var t = t_numer / denom;
    
        point.x = p0.x + (t * s10_x);
        point.y = p0.y + (t * s10_y);

        return true;
    }

    public static bool IsRectangleOverlap(Vector[] rect1, Vector[] rect2) {
        if (DoAxisSeparationTest(rect1[0], rect1[1], rect1[2], rect2)) return false;
        if (DoAxisSeparationTest(rect1[1], rect1[2], rect1[3], rect2)) return false;
        if (DoAxisSeparationTest(rect1[2], rect1[3], rect1[0], rect2)) return false;
        if (DoAxisSeparationTest(rect1[3], rect1[0], rect1[1], rect2)) return false;
        if (DoAxisSeparationTest(rect2[0], rect2[1], rect2[2], rect1)) return false;
        if (DoAxisSeparationTest(rect2[1], rect2[2], rect2[3], rect1)) return false;
        if (DoAxisSeparationTest(rect2[2], rect2[3], rect2[0], rect1)) return false;
        if (DoAxisSeparationTest(rect2[3], rect2[0], rect2[1], rect1)) return false;
        return true;
    }

    public static bool IsRectangleCircleOverlap(Vector rectOrigin, Vector rectSize, Number rectRotation, Vector circleOrigin, Number circleRadius) {
        circleOrigin -= rectOrigin;
        if (rectRotation != Number.Zero) {
            circleOrigin = Vector.Rotate(Vector.zero, circleOrigin, -rectRotation);
        }

        var corners = GetRectangleVertices(Vector.zero, rectSize, 0);
        circleOrigin.x = Number.Abs(circleOrigin.x);
        circleOrigin.y = Number.Abs(circleOrigin.y);
        var h = corners[1];
        var u = circleOrigin - h;
        if (u.x < 0) u.x = 0;
        if (u.y < 0) u.y = 0;

        return Vector.Dot(u, u) <= (circleRadius * circleRadius);
    }

    public static Vector[] GetRectangleVertices(Vector origin, Vector size, Number rotation) {
        var vertices = new Vector[4];
        vertices[0].x = -size.x; vertices[0].y = size.y;  // top-left
        vertices[1].x = size.x;  vertices[1].y = size.y;  // top-right
        vertices[2].x = size.x;  vertices[2].y = -size.y; // bottom-right
        vertices[3].x = -size.x; vertices[3].y = -size.y; // bottom-left
        for (int i = 0; i < 4; i++) {
            if (rotation != Number.Zero) {
                vertices[i] = Vector.Rotate(Vector.zero, vertices[i], rotation);
            }
            vertices[i] += origin;
        }
        return vertices;
    }

    public static bool IsRectangleOverlapMoreThanHalfOnX(BoxCollider b1, BoxCollider b2) {
        Number x0, x1, x2, x3;
        if (b1.position.x + b1.size.x < b2.position.x + b2.size.x) {
            x0 = b1.position.x - b1.size.x;
            x1 = b1.position.x + b1.size.x;
            x2 = b2.position.x - b2.size.x;
            x3 = b2.position.x + b2.size.x;
        } else {
            x0 = b2.position.x - b2.size.x;
            x1 = b2.position.x + b2.size.x;
            x2 = b1.position.x - b1.size.x;
            x3 = b1.position.x + b1.size.x;
        }
        var len = Math.Abs(x1 - x2);
        return len >= b1.size.x || len >= b2.size.x;
    }

/// <summary>
/// Does axis separation test for a convex quadrilateral.
/// </summary>
/// <param name="x1">Defines together with x2 the edge of quad1 to be checked whether its a separating axis.</param>
/// <param name="x2">Defines together with x1 the edge of quad1 to be checked whether its a separating axis.</param>
/// <param name="x3">One of the remaining two points of quad1.</param>
/// <param name="otherQuadPoints">The four points of the other quad.</param>
/// <returns>Returns <c>true</c>, if the specified edge is a separating axis (and the quadrilaterals therefor don't
/// intersect). Returns <c>false</c>, if it's not a separating axis.</returns>
    static bool DoAxisSeparationTest(Vector x1, Vector x2, Vector x3, Vector[] otherQuadPoints) {
        var vec = x2 - x1;
        var rotated = new Vector(-vec.y, vec.x);

        bool refSide = (rotated.x * (x3.x - x1.x) + rotated.y * (x3.y - x1.y)) >= 0;

        foreach (var pt in otherQuadPoints) {
            bool side = (rotated.x * (pt.x - x1.x) + rotated.y * (pt.y - x1.y)) >= 0;
            if (side == refSide) {
                // At least one point of the other quad is one the same side as x3. Therefor the specified edge can't be a
                // separating axis anymore.
                return false;
            }
        }
        // All points of the other quad are on the other side of the edge. Therefor the edge is a separating axis and
        // the quads don't intersect.
        return true;
    }
/*
    public static bool IsPointInRectangle(Vector point, Vector[] vertices) {
        var ap = vertices[0] - point;
        var ab = vertices[0] - vertices[1];
        var ad = vertices[0] - vertices[3];

        var r1 = Vector.Dot(ap, ab);
        var r2 = Vector.Dot(ab, ab);
        var r3 = Vector.Dot(ap, ad);
        var r4 = Vector.Dot(ad, ad);

        return 0 <= r1 && r1 <= r2 && 0 <= r3 && r3 <= r4;
    }

    public static bool IsLineInCircle(Vector p1, Vector p2, Vector circleOrigin, Number radius) {
        return false;
    }

    // stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
    public static bool IsRectangleCircleOverlap(Vector[] vertices, Vector circleOrigin, Number radius) {
        return IsPointInRectangle(circleOrigin, vertices) ||
               IsLineInCircle(vertices[0], vertices[1], circleOrigin, radius) ||
               IsLineInCircle(vertices[1], vertices[2], circleOrigin, radius) ||
               IsLineInCircle(vertices[2], vertices[3], circleOrigin, radius) ||
               IsLineInCircle(vertices[3], vertices[0], circleOrigin, radius);
    }
*/
/*
    public static bool IsPointInRectangleAligned(Vector point, Vector origin, Vector size) {
        return (point.x >= origin.x - size.x) && (point.x <= origin.x + size.x) &&
               (point.y >= origin.y - size.y) && (point.y <= origin.y + size.y);
    }

    public static bool IsRectangleOverlapAligned(Vector origin, Vector size, Vector[] corners, Vector offset) {
        for (int i = 0; i < corners.Length; i++) {
            if (IsPointInRectangleAligned(origin, size, corners[i] + offset)) {
                return true;
            }
        }
        return false;
    }

    public static bool IsRectangleOverlap(Vector origin, Vector size, Vector[] corners, Vector offset, Number rotation) {
        var rotatedCorners = GetRectangleVertices(origin, size, rotation);
        var topLeft = Vector.Rotate(Vector.zero, corners[0] + offset, rotation);
        var topRight = Vector.Rotate(Vector.zero, corners[1] + offset, rotation);
        var bottomRight = Vector.Rotate(Vector.zero, corners[2] + offset, rotation);
        var bottomLeft = Vector.Rotate(Vector.zero, corners[3] + offset, rotation);
        Vector newOrigin;
        Vector newSize;
        newOrigin.x = (topLeft.x + topRight.x) / 2;
        newOrigin.y = (topLeft.y + bottomLeft.y) / 2;
        newSize.x = (topRight.x - topLeft.x) / 2;
        newSize.y = (topLeft.y - bottomLeft.y) / 2;
        return IsRectangleOverlapAligned(newOrigin, newSize, rotatedCorners, Vector.zero);
    }
*/
}

}