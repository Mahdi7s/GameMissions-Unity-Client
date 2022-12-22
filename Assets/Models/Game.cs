using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PackageName { get; set; }
    public int Order { get; set; }
    public int NextRewardedVideoTimeout { get; set; }
    public int RewardedVideoReward { get; set; }
    public int IntrestitialPerLevel { get; set; }
    public string Description { get; set; }
}
