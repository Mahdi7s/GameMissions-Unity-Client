using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
    public MissionType MissionType { get; set; }
    public int CompletionLevel { get; set; }
    public string? Title { get; set; }
    public int Order { get; set; }
    public int Reward { get; set; }
    public string? Description { get; set; }
}
