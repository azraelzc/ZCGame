
using System.Collections.Generic;

namespace Battle.Core {

public class SphereCollider : Collider {

    public Number radius { get; private set; }

    public SphereCollider(Number radius) {
        this.radius = radius;
    }

    public override bool OverlapBox(Vector origin, Vector size, Number rotation) {
        return PhysicsUtils.IsRectangleCircleOverlap(origin, size, rotation, this.position, this.radius);
    }

    public override bool OverlapCircle(Vector origin, Number radius) {
        return Vector.Distance(origin, this.position) <= (radius + this.radius);
    }

}

}