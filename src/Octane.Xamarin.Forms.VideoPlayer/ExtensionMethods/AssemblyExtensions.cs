using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Octane.Xamarin.Forms.VideoPlayer.ExtensionMethods
{
    /// <summary>
    /// Assembly extension methods.
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Determines whether the assembly contains the specified resource name.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">The name of the embedded resource.</param>
        /// <returns></returns>
        public static bool ContainsManifestResource(this Assembly assembly, string resourceName)
        {
            return assembly != null
                && assembly.GetManifestResourceNames()
                .Any(x => x.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource stream.</returns>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static Stream GetEmbeddedResourceStream(this Assembly assembly, string resourceFileName)
        {
            var resourceNames = assembly.GetManifestResourceNames();

            var resourcePaths = resourceNames
                .Where(x => x.EndsWith(resourceFileName, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (!resourcePaths.Any())
            {
                throw new Exception($"Resource ending with {resourceFileName} not found.");
            }

            if (resourcePaths.Count() > 1)
            {
                throw new Exception($"Multiple resources ending with {resourceFileName} found: {Environment.NewLine}{string.Join(Environment.NewLine, resourcePaths)}");
            }

            return assembly.GetManifestResourceStream(resourcePaths.Single());
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource as a byte array.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static byte[] GetEmbeddedResourceBytes(this Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource as a string.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static string GetEmbeddedResourceString(this Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
