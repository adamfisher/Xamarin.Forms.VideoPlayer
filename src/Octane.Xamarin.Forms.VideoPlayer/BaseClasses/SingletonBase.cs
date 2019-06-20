using System.Diagnostics;

namespace Octane.Xamarin.Forms.VideoPlayer.BaseClasses
{
    /// <summary>
    /// Singleton Base Class.
    /// Allows the use of a single Static Instance on Demand.
    /// </summary>
    /// <typeparam name="T">The type of object to make a singleton.</typeparam>
    /// <typeparam name="TI">The interface of the Type in T. </typeparam>
    [DebuggerStepThrough]
    public abstract class SingletonBase<T> where T : new()
    {
        /// <summary>
        /// Concurrency Locking Object.
        /// </summary>
        protected static readonly object _ConcurrencyLock = new object();

        /// <summary>
        /// Holds the current active singleton object.
        /// </summary>
        private static T _Current;

        /// <summary>
        /// Gets or sets the Singleton Current Static Property.
        /// </summary>
        public static T Current
        {
            get
            {
                if (_Current == null)
                {
                    lock (_ConcurrencyLock)
                    {
                        if (_Current == null)
                        {
                            _Current = new T();
                        }
                    }
                }

                return _Current;
            }
            set
            {
                lock (_ConcurrencyLock)
                {
                    _Current = value;
                }
            }
        }
    }
}