namespace Battle.Core {

public class Collider {

    public Entity entity { get; private set; }
    public Vector offset { get; private set; }
    public Vector position { get { return this.entity.position + offset; } }
    public Number rotation { get { return this.entity.rotation; } }
    public Vector size { get; protected set; } // AABB
    public bool isStatic { get; set; }

    public Collider() {
        this.offset = Vector.zero;
        this.isStatic = false;
    }

    public void SetOffset(Vector offset) {
        this.offset = offset;
    }

    internal void SetEntity(Entity entity) {
        this.entity = entity;
    }

    public virtual bool IsConcrete() {
        return true;
    }

    public virtual bool IsTouching(Collider c) {
        return false;
    }

    public virtual bool OverlapBox(Vector origin, Vector size, Number rotation) {
        return false;
    }

    public virtual bool OverlapCircle(Vector origin, Number radius) {
        return false;
    }

}

}