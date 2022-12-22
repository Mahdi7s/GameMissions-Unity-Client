using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerRequest
{
    public const string Route = "/Players";

    public string DeviceId { get; set; }
    public string GamePackageName { get; set; }
}
