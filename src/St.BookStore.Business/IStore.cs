using St.BookStore.Core.Entities;

/// <summary>
/// The store service
/// </summary>
public interface IStore
{
    /// <summary>
    /// Loads data from json
    /// </summary>
    void Import(string catalogAsJson);

    /// <summary>
    /// Gets stock quantity by product name
    /// </summary>
    int Quantity(string name);

    /// <summary>
    /// Computes basket items
    /// </summary>
    double Buy(params string[] basketByNames);
}