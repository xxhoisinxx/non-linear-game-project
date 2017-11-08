/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace _3DPrimitives {
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sector.
    /// </summary>
    public class Sector : IEquatable<Sector> {
        /// <summary>
        /// The mesh filter.
        /// </summary>
        private readonly MeshFilter meshFilter;

        /// <summary>
        /// The mesh collider.
        /// </summary>
        private readonly MeshCollider meshCollider;

        /// <summary>
        /// The depth.
        /// </summary>
        private float depth;

        /// <summary>
        /// The height.
        /// </summary>
        private float height;

        /// <summary>
        /// The slice.
        /// </summary>
        private int slice;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sector"/> class.
        /// </summary>
        /// <param name="mf">
        /// The mf.
        /// </param>
        /// <param name="mc">
        /// The mc.
        /// </param>
        /// <param name="transform">
        /// The transform.
        /// </param>
        public Sector(MeshFilter mf, MeshCollider mc, Transform transform) {
            this.meshFilter = mf;
            this.meshCollider = mc;
            this.Transform = transform;
        }

        /// <summary>
        /// Gets or sets the Depth.
        /// </summary>
        public float Depth {
            get {
                return this.depth;
            }

            set {
                this.depth = value;
                this.AttachMesh();
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public float Height {
            get {
                return this.height;
            }

            set {
                this.height = value;
                this.AttachMesh();
            }
        }

        /// <summary>
        /// Gets or sets the Slice.
        /// </summary>
        public int Slice {
            get {
                return this.slice;
            }

            set {
                this.slice = Mathf.Clamp(value, 3, int.MaxValue);
                this.AttachMesh();
            }
        }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        public Mesh Mesh {
            get {
                var height = this.Height / 2;
                var points = new[]
                {
                    Matrix4x4.Rotate(Quaternion.Euler(0, -180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, height, 0)),
                    Matrix4x4.Rotate(Quaternion.Euler(0, -180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, height, this.Depth)),
                    Matrix4x4.Rotate(Quaternion.Euler(0, 180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, height, this.Depth)),
                    Matrix4x4.Rotate(Quaternion.Euler(0, -180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, -height, this.Depth)),
                    Matrix4x4.Rotate(Quaternion.Euler(0, -180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, -height, 0)),
                    Matrix4x4.Rotate(Quaternion.Euler(0, 180.0f / this.Slice, 0))
                        .MultiplyPoint3x4(new Vector3(0, -height, this.Depth)),
                };
                return new Mesh {
                    vertices = new[]
                    {
                        points[0],
                        points[1],
                        points[2],
                        ////////////////////////////////////////////////////////////////////
                        points[1],
                        points[0],
                        points[3],
                        ////////////////////////////////////////////////////////////////////
                        points[3],
                        points[0],
                        points[4],
                        ////////////////////////////////////////////////////////////////////
                        points[0],
                        points[2],
                        points[4],
                        ////////////////////////////////////////////////////////////////////
                        points[4],
                        points[2],
                        points[5],
                        ////////////////////////////////////////////////////////////////////
                        points[4],
                        points[5],
                        points[3],
                        ////////////////////////////////////////////////////////////////////
                        points[5],
                        points[2],
                        points[1],
                        /////////////////////////////////////////////////////////////////////
                        points[5],
                        points[1],
                        points[3],
                        ///////////////////////////////////////////////////////////////////
                    },
                    uv = new[]
                    {
                        new Vector2(0.497f, 0.856f),
                        new Vector2(0.742f, 0.998f),
                        new Vector2(0.497f, 0.573f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.435f, 0.569f),
                        new Vector2(0.68f, 0.569f),
                        new Vector2(0.435f, 0.002f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.68f, 0.002f),
                        new Vector2(0.68f, 0.569f),
                        new Vector2(0.435f, 0.002f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.687f, 0.569f),
                        new Vector2(0.972f, 0.569f),
                        new Vector2(0.687f, 0.002f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.687f, 0.002f),
                        new Vector2(0.972f, 0.569f),
                        new Vector2(0.971f, 0.002f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.994f, 0.856f),
                        new Vector2(0.994f, 0.573f),
                        new Vector2(0.748f, 0.998f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.427f, 0.569f),
                        new Vector2(0.427f, 0.002f),
                        new Vector2(0.002f, 0.002f),
                        ////////////////////////////////////////////////////////////////////
                        new Vector2(0.427f, 0.569f),
                        new Vector2(0.002f, 0.002f),
                        new Vector2(0.002f, 0.569f),
                    },
                    triangles = new[]
                        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }
                };
            }
        }

        public static bool operator ==(Sector sector1, Sector sector2) => EqualityComparer<Sector>.Default.Equals(sector1, sector2);

        public static bool operator !=(Sector sector1, Sector sector2) => !(sector1 == sector2);

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            return obj.GetType() == this.GetType() && this.Equals((Sector)obj);
        }

        public bool Equals(Sector other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Math.Abs(this.Depth - other.Depth) < float.Epsilon
                   && Math.Abs(this.Height - other.Height) < float.Epsilon
                   && this.Slice == other.Slice
                   && this.meshFilter == other.meshFilter
                   && this.meshCollider == other.meshCollider
                   && this.Transform == other.Transform;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode() {
            var hashCode = -313245439;
            hashCode = (hashCode * -1521134295) + EqualityComparer<MeshFilter>.Default.GetHashCode(this.meshFilter);
            hashCode = (hashCode * -1521134295) + EqualityComparer<MeshCollider>.Default.GetHashCode(this.meshCollider);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Transform>.Default.GetHashCode(this.Transform);
            return hashCode;
        }

        /// <summary>
        /// The reinitialize.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="slice">
        /// The slice.
        /// </param>
        public void Reinitialize(float depth, float height, int slice) {
            this.depth = depth;
            this.height = height;
            this.slice = slice;
            this.AttachMesh();
        }

        /// <summary>
        /// The attach mesh.
        /// </summary>
        private void AttachMesh() {
            var mesh = this.Mesh;
            this.meshFilter.sharedMesh = mesh;
            this.meshCollider.sharedMesh = mesh;
        }
    }
}
