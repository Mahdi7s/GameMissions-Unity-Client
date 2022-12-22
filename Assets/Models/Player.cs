using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

public class Player
{
  public int Id { get; set; }
  public string? DeviceId { get; set; }
  public string? LastConnectedIP { get; set; }
  public string LocaleCode { get; set; } = "en";
  public int GameId { get; set; }
  public bool Rated { get; set; }
  public int Level { get; set; }
  public DateTime LastAdWatch { get; set; }
}
