using UnityEngine;
using System.Collections;

namespace HLNetwork
{
    public class ImagePositionCache
    {

        #region Fields

        /// <summary>
        /// Stores the ImagePositions
        /// </summary>
        private Queue _cachedPositions;

        /// <summary>
        /// The desired age of ImagePositions returned by GetDelayedPosition
        /// </summary>
        private readonly float _delayTime;

        #endregion

        public ImagePositionCache(float delayTime)
        {
            _delayTime = delayTime;
            _cachedPositions = Queue.Synchronized(new Queue());
        }

        #region Public Methods

        /// <summary>
        /// Stores the current position.  Should be called once per frame.
        /// </summary>
        public void Update()
        {

            ///
            /// Add the current ImagePosition to the queue
            ///

            ImagePosition newPosition = new HLNetwork.ImagePosition(Camera.main.transform);
            _cachedPositions.Enqueue(newPosition);

            ///
            /// Get rid of ImagePositions which are too old to be useful
            ///

            float oldestTimeToKeep = newPosition.TimeCreated - _delayTime;
            while ((_cachedPositions.Peek() as ImagePosition).TimeCreated < oldestTimeToKeep)
            {
                _cachedPositions.Dequeue();
            }

        }

        /// <summary>
        /// Gets the earliest available position obtained after _delayTime ago.
        /// Assumes that Update has been called recently.
        /// </summary>
        /// <returns></returns>
        public ImagePosition GetDelayedPosition()
        {
            if (_cachedPositions.Count > 0)
            {
                return _cachedPositions.Peek() as ImagePosition;
            }
            else
            {
                throw new System.InvalidOperationException
                    ("ImagePositionCache.GetDelayedPosition read empty cache, was possibly called without Update shortly beforehand");
            }
        }

        #endregion

    }
}