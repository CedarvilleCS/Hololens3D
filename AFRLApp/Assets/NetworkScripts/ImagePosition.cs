using UnityEngine;
using System.Collections;

namespace HLNetwork
{

    /// <summary>
    /// Represents the camera's position and rotation in space at a
    /// particular moment
    /// </summary>
    public class ImagePosition
    {

        #region Static Variables

        /// <summary>
        /// Used within this class to assign all instances unique id's
        /// </summary>
        private static int nextId = 0;

        #endregion

        #region Fields

        /// <summary>
        /// Unique identifier for references over the network
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// Position of user in world space at the time of this object's creation
        /// </summary>
        public readonly Vector3 Position;

        /// <summary>
        /// Direction user was facing at the time of this object's creation
        /// </summary>
        public readonly Vector3 Forward;

        /// <summary>
        /// Upward direction relative to user at the time of this object's creation
        /// </summary>
        public readonly Vector3 Up;

        /// <summary>
        /// Time since application launch of this object's creation
        /// </summary>
        public readonly float TimeCreated;

        #endregion

        #region Constructor

        public ImagePosition(Transform transform)
        {
            ID = nextId++;
            Position = transform.position;
            Forward = transform.forward;
            Up = transform.up;
            TimeCreated = Time.time;
        }

        #endregion

    }

}
