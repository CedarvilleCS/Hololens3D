using UnityEngine;
using System.Collections;

namespace HLNetwork
{
    public class ImagePosition
    {

        #region Static Variables

        private static int nextId = 0;

        #endregion

        #region Fields

        /// <summary>
        /// Unique identifier for references over the network
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Position of user in world space at the time of this object's creation
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Direction user was facing at the time of this object's creation
        /// </summary>
        Vector3 Forward { get; }

        /// <summary>
        /// Upward direction relative to user at the time of this object's creation
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// Time since application launch of this object's creation
        /// </summary>
        float TimeCreated { get; }

        #endregion

        #region Constructor

        public ImagePosition()
        {
            ID = nextId++;
            Forward = Camera.main.transform.forward;
            Up = Camera.main.transform.up;
            TimeCreated = Time.time;
        }

        #endregion

    }
}
