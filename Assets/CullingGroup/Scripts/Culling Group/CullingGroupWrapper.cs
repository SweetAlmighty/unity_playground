using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Virbela.CodingTest.Utilities
{
    /// <summary>
    /// Wrapper class around <see cref="UnityEngine.CullingGroup"/>.
    /// </summary>
    /// <typeparam name="T">
    /// A type that implements <see cref="ICullable"/>, which ensures that the expected properties needed to build
    /// reference <see cref="BoundingSphere"/> instances are available.
    /// </typeparam>
    public class CullingGroupWrapper<T> where T : ICullable
    {
        /// <summary>
        /// <see cref="CullingGroup"/> instance used to create an efficient pool of <see cref="ICullable"/> objects to
        /// pull from.
        /// </summary>
        private CullingGroup cullingGroup;

        /// <summary>
        /// An array that will be filled with indices of <see cref="BoundingSphere"/> instances within
        /// <see cref="cullingGroup"/> that represent the pool used to find the <see cref="ICullable"/> closest to a
        /// given reference <see cref="Transform"/>.
        /// </summary>
        private int[] indices = Enumerable.Empty<int>().ToArray();

        /// <summary>
        /// Default constructor, initializes the internal <see cref="CullingGroup"/> instance, sets the camera to
        /// <see cref="Camera.main"/>, and sets the distance(s) to be used when checking for objects in the scene.
        /// </summary>
        /// <param name="objectCount">
        /// The amount of objects that are in the scene when the <see cref="CullingGroup"/> is being created.
        /// </param>
        public CullingGroupWrapper(int objectCount)
        {
            this.ResetIndices(objectCount);
            this.cullingGroup = new CullingGroup();
            this.cullingGroup.targetCamera = Camera.main;
            this.cullingGroup.SetBoundingDistances(new [] { 0f, float.PositiveInfinity });
        }

        /// <summary>
        /// Set the reference point from which distance bands are measured.
        /// </summary>
        /// <param name="transform">The reference point in the scene that will be used.</param>
        public void SetDistanceReferencePoint(Transform transform)
            => this.cullingGroup.SetDistanceReferencePoint(transform);

        /// <summary>
        /// Sets the array of bounding sphere definitions that the CullingGroupWrapper should compute culling for.
        /// </summary>
        /// <param name="objects">
        /// The array of objects that reference <see cref="BoundingSphere"/>s will be created for.
        /// </param>
        public void SetBoundingSpheres(T[] objects)
        {
            this.ResetIndices(objects.Length);
            this.cullingGroup.SetBoundingSpheres(objects.Select(this.CreateBoundingSphere).ToArray());
            this.cullingGroup.SetBoundingSphereCount(objects.Length);
        }

        /// <summary>
        /// Queries the internal <see cref="UnityEngine.CullingGroup"/> to find the positions of objects within the scene
        /// that are within the distance band indicated by <paramref name="distanceIndex"/>.
        /// </summary>
        /// <param name="distanceIndex">The distance band that retrieved <see cref="BoundingSphere"/>s must be in.</param>
        /// <param name="firstIndex">The index of the <see cref="BoundingSphere"/> to begin searching at.</param>
        /// <returns>
        /// A collection of indices that represent the objects within the internal <see cref="UnityEngine.CullingGroup"/>'s
        /// distance band.
        /// </returns>
        public IEnumerable<int> FindClosestObjects(int distanceIndex = 0, int firstIndex = 0)
        {
            this.cullingGroup.QueryIndices(distanceIndex, this.indices, firstIndex);
            return this.indices;
        }
        
        /// <summary>
        /// Disposes of <see cref="cullingGroup"/> and flags it for garbage collection.
        /// </summary>
        public void Destroy()
        {
            this.cullingGroup.Dispose();
            this.cullingGroup = null;
        }

        /// <summary>
        /// Helper function that resets the indices used by <see cref="cullingGroup"/>.
        /// </summary>
        /// <param name="objectCount">The amount of objects in the scene that will be tracked and sorted.</param>
        private void ResetIndices(int objectCount)
        {
            Array.Resize(ref this.indices, objectCount);
            for (int i = this.indices.Length - 1; i >= 0; i--)
                this.indices[i] = i;
        }

        /// <summary>
        /// Helper function used to create a <see cref="BoundingSphere"/> representation of <paramref name="cullable"/>.
        /// </summary>
        /// <param name="cullable">
        /// The instance of <see cref="ICullable"/> that is having a <see cref="BoundingSphere"/> created that will be
        /// used by visibility calculations within the <see cref="cullingGroup"/>.
        /// </param>
        /// <returns>
        /// The newly created <see cref="BoundingSphere"/> representation of <paramref name="cullable"/>.
        /// </returns>
        private BoundingSphere CreateBoundingSphere(T cullable)
            => new BoundingSphere(cullable.Transform.position, cullable.Extents.x);
    }
}
