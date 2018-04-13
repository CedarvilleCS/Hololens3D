using UnityEngine;
using System.Collections;
using System;

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
            Forward = transform.forward.normalized;
            Up = transform.up.normalized;
            TimeCreated = Time.time;
        }

        public ImagePosition(int ID, Vector3 Position, Vector3 Forward, Vector3 Up, float TimeCreated)
        {
            this.ID = ID;
            this.Position = Position;
            this.Forward = Forward;
            this.Up = Up;
            this.TimeCreated = TimeCreated;
        }

        public ImagePosition()
        {
            ID = 0;
            this.Position = new Vector3(0, 0, 0);
            this.Forward = new Vector3(0, 0, 0);
            this.Up = new Vector3(0, 0, 0);
            this.TimeCreated = 0;
        }

        #endregion

        //To byte array of length 44
        public byte[] ToByteArray()
        {
            byte[] idBytes = BitConverter.GetBytes(ID);
            byte[] positionBytes = VectorToBytes(Position);
            byte[] forwardBytes = VectorToBytes(Forward);
            byte[] upBytes = VectorToBytes(Up);
            byte[] timeCreatedBytes = BitConverter.GetBytes(TimeCreated);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(idBytes);
                Array.Reverse(positionBytes);
                Array.Reverse(forwardBytes);
                Array.Reverse(upBytes);
                Array.Reverse(timeCreatedBytes);
            }
            byte[] finalArray = new byte[44];
            Array.Copy(idBytes, 0, finalArray, 0, 4);
            Array.Copy(positionBytes, 0, finalArray, 4, 12);
            Array.Copy(forwardBytes, 0, finalArray, 16, 12);
            Array.Copy(upBytes, 0, finalArray, 28, 12);
            Array.Copy(timeCreatedBytes, 0, finalArray, 40, 4);
            return finalArray;
        }

        public static ImagePosition FromByteArray(byte[] bytes)
        {
            byte[] idBytes = new byte[4];
            byte[] positionBytes = new byte[12];
            byte[] forwardBytes = new byte[12];
            byte[] upBytes = new byte[12];
            byte[] timeCreatedBytes = new byte[4];
            Buffer.BlockCopy(bytes, 0, idBytes, 0, 4);
            Buffer.BlockCopy(bytes, 4, idBytes, 0, 12);
            Buffer.BlockCopy(bytes, 16, idBytes, 0, 12);
            Buffer.BlockCopy(bytes, 28, idBytes, 0, 12);
            Buffer.BlockCopy(bytes, 40, idBytes, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(idBytes);
                Array.Reverse(positionBytes);
                Array.Reverse(forwardBytes);
                Array.Reverse(upBytes);
                Array.Reverse(timeCreatedBytes);
            }
            int id = BitConverter.ToInt32(idBytes, 0);
            Vector3 position = VectorFromBytes(positionBytes);
            Vector3 forward = VectorFromBytes(forwardBytes);
            Vector3 up = VectorFromBytes(upBytes);
            float timeCreated = BitConverter.ToSingle(timeCreatedBytes, 0);
            return new ImagePosition(id, position, forward, up, timeCreated);
        }

        //return byte array of size 12
        private byte[] VectorToBytes(Vector3 vect)
        {
            byte[] buff = new byte[sizeof(float) * 3];
            Buffer.BlockCopy(BitConverter.GetBytes(vect.x), 0, buff, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(vect.y), 0, buff, 1 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(vect.z), 0, buff, 2 * sizeof(float), sizeof(float));
            return buff;
        }

        //must be given a byte array of size 12 or will crash
        private static Vector3 VectorFromBytes(byte[] bytes)
        {
            Vector3 vect = Vector3.zero;
            vect.x = BitConverter.ToSingle(bytes, 0 * sizeof(float));
            vect.y = BitConverter.ToSingle(bytes, 1 * sizeof(float));
            vect.z = BitConverter.ToSingle(bytes, 2 * sizeof(float));
            return vect;
        }

    }

}
