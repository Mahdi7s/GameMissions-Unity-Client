using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerRequest
{
    public const string Route = "/Players";

    public int Id { get; set; }
    public string? DeviceId { get; set; }

    public string? LastConnectedIP { get; set; }
    public string LocaleCode { get; set; }
    public int GameId { get; set; }
    public bool Rated { get; set; }
    public int Level { get; set; }
    public DateTime LastAdWatch { get; set; }
}
