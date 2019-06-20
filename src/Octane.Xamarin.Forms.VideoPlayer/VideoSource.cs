using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Octane.Xamarin.Forms.VideoPlayer.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer.ExtensionMethods;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// Loads videos from files or the web.
    /// </summary>
    [TypeConverter(typeof(VideoSourceConverter))]
    public abstract class VideoSource : Element, IEquatable<VideoSource>
    {
        #region Fields

        /// <summary>
        /// The synchronize handle
        /// </summary>
        private readonly object _syncHandle = new object();

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// The completion source
        /// </summary>
        private TaskCompletionSource<bool> _completionSource;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the CancellationTokenSource.
        /// </summary>
        /// <remarks>
        /// Used by inheritors to implement cancellable loads.
        /// </remarks>
        protected CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return _cancellationTokenSource;
            }
            private set
            {
                if (_cancellationTokenSource == value)
                    return;

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = value;
            }
        }

        /// <summary>
        /// Determines whether a video is loading.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        private bool IsLoading => _cancellationTokenSource != null;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the video source changes.
        /// </summary>
        internal event EventHandler SourceChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSource" /> class.
        /// </summary>
        internal VideoSource()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows implicit casting from a string that represents an absolute URI.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static implicit operator VideoSource(string source)
        {
            if (!Uri.TryCreate(source, UriKind.Absolute, out var result) || result.Scheme == "file")
                return FromFile(source);

            return FromUri(result);
        }

        /// <summary>
        /// Allows implicit casting from <see cref="T:System.Uri" /> objects that were created with an absolute URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static implicit operator VideoSource(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("uri is relative");
           
            return FromUri(uri);
        }

        /// <summary>
        /// Called by inheritors to indicate that the source changed.
        /// </summary>
        protected void OnSourceChanged()
        {
            SourceChanged.RaiseEvent(this);
        }
        
        /// <summary>
        /// Returns a new <see cref="T:Xamarin.Forms.FileVideoSource"/> that reads from <paramref name="file"/>.
        /// </summary>
        public static VideoSource FromFile(string file)
        {
            return new FileVideoSource()
            {
                File = file
            };
        }

        /// <summary>
        /// Returns a new <see cref="T:Xamarin.Forms.StreamVideoSource" /> that reads from <paramref name="stream" />.
        /// </summary>
        /// <param name="stream">The lambda responsible for extracting the stream.</param>
		/// <param name="format">The stream container format (i.e. "mp4", "webm", etc.)</param>
        /// <returns></returns>
		public static VideoSource FromStream(Func<Stream> stream, string format)
        {
            return new StreamVideoSource()
            {
				Format = format,
                Stream = token => Task.Run(stream, token)
            };
        }

        /// <summary>
        /// Creates an VideoSource for an EmbeddedResource included in an assembly.
        /// Checks the assembly specified by the <param name="assembly"> parameter first if specified,
        /// then in the assembly from which the call to FromResource is made,
        /// and finally attempts to find it inside the entry assembly.
        /// </summary>
        /// <param name="resource">A string representing the id of the EmbeddedResource to load.</param>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <returns>
        /// A newly created VideoSource. Null is returned if no resource is specified and an exception is thrown 
		/// if no file extension is found for the resource.
        /// </returns>
        public static VideoSource FromResource(string resource, Assembly assembly = default(Assembly))
        {
			if (string.IsNullOrEmpty (resource))
				return null;
			
			if (!Path.HasExtension (resource))
				throw new Exception ($"The specified resource '{resource}' must contain a valid file extension.");

            var foundResource = false;
			var format = Path.GetExtension (resource)?.Replace (".", string.Empty);
            Assembly parameterAssembly = null, callingAssembly = null, entryAssembly = null;

            if (assembly != null)
            {
                parameterAssembly = assembly;
                foundResource = assembly.ContainsManifestResource(resource);
            }

            if (!foundResource)
            {
                assembly = Assembly.GetCallingAssembly();
                callingAssembly = assembly;
                foundResource = assembly.ContainsManifestResource(resource);
            }

            if (!foundResource)
            {
                assembly = Assembly.GetEntryAssembly();
                entryAssembly = assembly;
                foundResource = assembly.ContainsManifestResource(resource);
            }
            
            if (!foundResource)
            {
                var resourceNames = new List<string>();

                if (parameterAssembly != null)
                    resourceNames.AddRange(parameterAssembly.GetManifestResourceNames());
                if (callingAssembly != null)
                    resourceNames.AddRange(callingAssembly.GetManifestResourceNames());
                if (entryAssembly != null)
                    resourceNames.AddRange(entryAssembly.GetManifestResourceNames());

                Log.Error($"Unable to locate the embedded resource '{resource}'. " +
                          $"Possible candidates are: {Environment.NewLine}{string.Join(Environment.NewLine, resourceNames)}");

                return null;
            }

			return FromStream(() => assembly.GetEmbeddedResourceStream(resource), format);
        }

		/// <summary>
		/// Returns a new <see cref="T:Xamarin.Forms.UriVideoSource" /> that reads from <paramref name="uri" />.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">uri is relative</exception>
		public static VideoSource FromUri(string uri)
		{
			return FromUri (new Uri (uri));
		}

        /// <summary>
        /// Returns a new <see cref="T:Xamarin.Forms.UriVideoSource" /> that reads from <paramref name="uri" />.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">uri is relative</exception>
        public static VideoSource FromUri(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("uri is relative");

            return new UriVideoSource()
            {
                Uri = uri
            };
        }

        /// <summary>
        /// Called by inheritors to indicate the beginning of a loading operation.
        /// </summary>
        /// 
        /// <remarks>
        /// OnLoadingCompleted should follow a OnLoadingStarted.
        /// </remarks>
        protected void OnLoadingStarted()
        {
            var obj = _syncHandle;
            var lockTaken = false;

            try
            {
                Monitor.Enter(obj, ref lockTaken);
                CancellationTokenSource = new CancellationTokenSource();
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(obj);
            }
        }

        /// <param name="cancelled">A bool indicating if the source was cancelled.</param>
        /// <summary>
        /// Called by inheritors to indicate the end of the loading of the source.
        /// </summary>
        /// 
        /// <remarks>
        /// OnLoadingCompleted should follow a OnLoadingStarted.
        /// </remarks>
        protected void OnLoadingCompleted(bool cancelled)
        {
            if (!IsLoading || this._completionSource == null)
                return;

            var completionSource = Interlocked.Exchange(ref this._completionSource, null);
            completionSource?.SetResult(cancelled);

            var obj = _syncHandle;
            var lockTaken = false;

            try
            {
                Monitor.Enter(obj, ref lockTaken);
                CancellationTokenSource = null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(obj);
            }
        }

        /// <summary>
        /// Request a cancel of the VideoSource loading.
        /// </summary>
        /// <remarks>
        /// Calling Cancel() multiple times will throw an exception.
        /// </remarks>
        /// <returns>
        /// An awaitable Task. The result of the Task indicates if the Task was successfully cancelled.
        /// </returns>
        public virtual Task<bool> Cancel()
        {
            if (!IsLoading)
                return Task.FromResult(false);

            var completionSource1 = new TaskCompletionSource<bool>();
            var completionSource2 = Interlocked.CompareExchange(ref _completionSource, completionSource1, null);

            if (completionSource2 == null)
                _cancellationTokenSource.Cancel();
            else
                completionSource1 = completionSource2;

            return completionSource1.Task;
        }

        /// <summary>
        /// Determines if two video sources are equivalent.
        /// </summary>
        /// <param name="other">The other video source.</param>
        /// <returns>True if the video sources are equal, false otherwise.</returns>
        public abstract bool Equals(VideoSource other);

        #endregion
    }
}
