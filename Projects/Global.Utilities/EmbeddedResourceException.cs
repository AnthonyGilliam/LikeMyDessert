using System;
using System.Collections.Generic;

namespace Global.Utilities
{
    public class EmbeddedResourceException : SystemException
    {
        private string _path;
        private string _filename;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public EmbeddedResourceException() : base() { }

        public EmbeddedResourceException(string message) : base(message) { }

        public EmbeddedResourceException(string message, Exception inner) : base(message, inner) { }
    }
}