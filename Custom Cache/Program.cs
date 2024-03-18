// See https://aka.ms/new-console-template for more information


IDataDownloader dataDownloader = new CachingDataDownloader(new SlowDataDownloader());

Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));


public interface IDataDownloader
{
    string DownloadData(string resourceId);
}

public class CachingDataDownloader : IDataDownloader
{
    private readonly Cache<string, string> _cache = new();
    private readonly IDataDownloader _dataDownloader;

    public CachingDataDownloader(IDataDownloader dataDownloader)
    {
        _dataDownloader = dataDownloader;
    }

    public string DownloadData(string resourceId)
    {
        return _cache.Get(resourceId, _dataDownloader.DownloadData);

    }
}


public class SlowDataDownloader : IDataDownloader
{

    public string DownloadData(string resourceId)
    {
        //let's imagine this method downloads real data,
        //and it does it slowly

        Thread.Sleep(1000);
        return $"Some data for {resourceId}";

    }
}



public class Cache<TKey, TValue>
{
    private Dictionary<TKey, TValue> _data = new();

    public TValue Get(TKey key, Func<TKey, TValue> GetDataForTheFirstTime)
    {
        if (!_data.ContainsKey(key))
        {
            _data[key] = GetDataForTheFirstTime(key);
        }

        return _data[key];
    }


}