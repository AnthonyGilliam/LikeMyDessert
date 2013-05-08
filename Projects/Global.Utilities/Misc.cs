using System;
using System.Collections.Generic;
using System.Reflection;

namespace Global.Utilities
{
    /// <summary>
    /// Holding area for methods until they can be grouped into a more meaningful
    /// class name with like methods.
    /// </summary>
    public static class Misc
    {
        /// <summary>
        /// Helper method for retrieving an embedded resource text file.
        /// </summary>
        /// <param name="pathAsNamespace">The namespace path for the file.</param>
        /// <param name="filename">The filename to be read.</param>
        /// <returns>A string of text that was read from the file.</returns>
        public static string ReadEmbeddedTextFile(
            Assembly assembly
            , string pathAsNamespace
            , string filename)
        {
            System.IO.Stream STREAM = assembly.GetManifestResourceStream(pathAsNamespace + "." + filename);
            if (STREAM != null)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(STREAM, true);
                return sr.ReadToEnd();
            }
            else
            {
                EmbeddedResourceException exception = new EmbeddedResourceException();
                exception.FileName = filename;
                exception.Path = pathAsNamespace;
                throw exception;
            }
        }

        /// <summary>
        /// Helper for reading an embedded binary file.
        /// </summary>
        /// <param name="pathAsNamespace">The namespace which represents the path
        /// of the file to be read.</param>
        /// <param name="filename">The filename of the file to be read.</param>
        /// <returns>The byte array representing the data that was read
        /// from the specified file.</returns>
        public static byte[] ReadEmbeddedBinaryFile(
            string pathAsNamespace
            , string filename
            , Type typeInAssembly)
        {
            System.IO.Stream STREAM = Assembly.GetAssembly(typeInAssembly).GetManifestResourceStream(pathAsNamespace + "." + filename);
            if (STREAM != null)
            {
                byte[] buffer = new byte[STREAM.Length];
                STREAM.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            else
            {
                throw new Exception("Could not find embedded resource.");
            }
        }

    }
}