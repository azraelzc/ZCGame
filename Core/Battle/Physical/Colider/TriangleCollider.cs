
namespace Battle.Core {

public class TriangleCollider : Collider {

    public TriangleCollider(Vector sz) :base() {
        this.size = sz;
    }

    public TriangleCollider(int sizeX, int sizeY) :this(new Vector(sizeX, sizeY)) {
    }
}

}