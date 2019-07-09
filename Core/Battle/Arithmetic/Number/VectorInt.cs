
namespace Battle {

    public struct VectorInt {
        public int x { get; set; }
        public int y { get; set; }

        public VectorInt(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Vector ToVector() {
            return (Vector)this;
        }

        public static VectorInt operator *(VectorInt self, int v) {
            return new VectorInt(self.x * v, self.y * v);
        }

        public static VectorInt operator /(VectorInt self, int v) {
            return new VectorInt(self.x / v, self.y / v);
        }

        public static VectorInt operator +(VectorInt self, VectorInt v) {
            return new VectorInt(self.x + v.x, self.y + v.y);
        }

        public static VectorInt operator -(VectorInt self, VectorInt v) {
            return new VectorInt(self.x - v.x, self.y - v.y);
        }

        public static implicit operator Vector(VectorInt self) {
            return new Vector(self.x, self.y);
        }
        public static Number SimpleDistance(VectorInt a, VectorInt b) {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }
    }

}
