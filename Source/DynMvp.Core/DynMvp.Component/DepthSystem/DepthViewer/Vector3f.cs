using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public class Vector3f
    {
        private float[] value = new float[3];

        public float this[int key]
        {
            get => value[key];
            set => this.value[key] = value;
        }

        public Vector3f()
        {
            value[0] = 0;
            value[1] = 0;
            value[2] = 0;
        }

        public Vector3f(float x, float y, float z)
        {
            value[0] = x;
            value[1] = y;
            value[2] = z;
        }

        public Vector3f Cross(Vector3f other)
        {
            return new Vector3f(value[1] * other[2] - value[2] * other[1],
                 value[2] * other[0] - value[0] * other[2],
                 value[0] * other[1] - value[1] * other[0]);
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(value[0] * value[0] + value[1] * value[1] + value[2] * value[2]);
        }

        public Vector3f Normalize()
        {
            float m = (float)Math.Sqrt(value[0] * value[0] + value[1] * value[1] + value[2] * value[2]);
            if (m != 0)
            {
                return new Vector3f(value[0] / m, value[1] / m, value[2] / m);
            }

            return new Vector3f(0, 0, 0);
        }

        public static Vector3f operator +(Vector3f value1, Vector3f value2)
        {
            return new Vector3f(value1[0] + value2[0], value1[1] + value2[1], value1[2] + value2[2]);
        }

        public static Vector3f operator *(Vector3f value1, float scale)
        {
            return new Vector3f(value1[0] * scale, value1[1] * scale, value1[2] * scale);
        }
    }
}
