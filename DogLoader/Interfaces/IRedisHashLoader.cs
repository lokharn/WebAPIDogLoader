namespace DogLoader.Interfaces
{
    /// <summary>
    /// Load service into Redis hash tables.
    /// </summary>
    public interface IRedisHashLoader
    {
        /// <summary>
        /// Set a hash table by the full set of parameters.
        /// </summary>
        /// <param name="tablename">The name of the hash table.</param>
        /// <param name="hashkey">The hash table key.</param>
        /// <param name="hashvalue">The hash table value.</param>
        /// <returns><see cref="Task"/> of set the hash table.</returns>
        Task SetHashTableAsync(string tablename, string hashkey, string hashvalue);
    }
}
