namespace Timelapse.Models;

public class Watch
{
    public Guid Id { get; set; }
    public string WatchBrand { get; set; }
    public string WatchName { get; set; }
    public double Price { get; set; }
    
    

    public Watch() {}
    public Watch(Guid id, string watchName, string watchBrand, double price)
    {
        this.Id = id;
        this.WatchName = watchName;
        this.WatchBrand = watchBrand;
        this.Price = price;
    }
}