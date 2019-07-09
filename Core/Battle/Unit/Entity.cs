namespace Battle.Core {
    public class Entity {
        public Number ID { get; private set; }
        public Vector position { get; private set; }
        public Number rotation { get; private set; }
        public Collider collider { get; private set; }
        public bool isDestroyed { get; private set; }
        public bool isEnabled { get; private set; }
        public bool isVisible { get; private set; }
    }
}
