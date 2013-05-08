namespace Global.Utilities.Interfaces
{
	/// <summary>
	/// Utilities for interfacing with the current HTTP context
	/// </summary>
	public interface IHttpHelper
	{
		/// <summary>
		/// Returns the fully qualified server path for a given virtual path
		/// </summary>
		/// <param name="virtualPath">Path starting at root directory</param>
		/// <returns></returns>
		string GetFullPath(string virtualPath);

		/// <summary>
		/// Adds an object to the Application Session.
		/// </summary>
		/// <param name="key">Object key</param>
		/// <param name="obj">Object to be added</param>
		void AddToAppSession(string key, object obj);

		/// <summary>
		/// Retrieves an object from the Application Session.
		/// </summary>
		/// <typeparam name="T">Retrieved object Type</typeparam>
		/// <param name="key">Session key</param>
		/// <returns></returns>
		T GetFromAppSession<T>(string key);

		/// <summary>
		/// Adds an object to the current request items.
		/// </summary>
		/// <param name="key">Object key</param>
		/// <param name="obj">Object to be added</param>
		void AddToCurrentRequestItems(string key, object obj);

		/// <summary>
		/// Retrieves an object from the current request items.
		/// </summary>
		/// <typeparam name="T">Retrieved object Type</typeparam>
		/// <param name="key">Request item key</param>
		/// <returns></returns>
		T GetFromCurrentRequestItems<T>(string key);

        /// <summary>
        /// Checks if a value exists for given session key
        /// </summary>
        /// <param name="key">Session key to query value</param>
        /// <returns></returns>
        bool ExistsInSession(string key);
    }
}