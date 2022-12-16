namespace Timelapse.Models;

public class Watches
{


 public Watches(int id, string watchName, string watchBrand){
    this.Id = id;
    this.WatchName = watchName;
    this.WatchBrand = watchBrand;}


     public int Id { get; set; }
    public string WatchBrand { get; set; }
    public string WatchName { get; set; }
}

// Path: TimelapseContext.cs
  



    

 

