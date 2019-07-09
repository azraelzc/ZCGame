
using System.Collections.Generic;

namespace Battle.Core {

public class CircleCollider : Collider {

    public Number radius { get; private set; }

    public CircleCollider(Number radius) {
        this.radius = radius;
        this.size = new Vector(radius, radius);
    }

    public override bool OverlapBox(Vector origin, Vector size, Number rotation) {
        return PhysicsUtils.IsRectangleCircleOverlap(origin, size, rotation, position, radius);
    }

    public override bool OverlapCircle(Vector origin, Number radius) {
        return Vector.Distance(origin, position) <= (radius + this.radius);
    }

}

}