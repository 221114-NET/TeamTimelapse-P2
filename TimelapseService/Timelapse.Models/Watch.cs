namespace Timelapse.Models;

public class Watch
{
    public Guid Id { get; set; }
    public string WatchBrand { get; set; }
    public string WatchName { get; set; }

    public Watch() {}
    public Watch(Guid id, string watchName, string watchBrand)
    {
        this.Id = id;
        this.WatchName = watchName;
        this.WatchBrand = watchBrand;
    }
}