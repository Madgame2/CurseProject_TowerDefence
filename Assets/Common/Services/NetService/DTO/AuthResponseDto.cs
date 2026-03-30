using System;
using UnityEngine;

[Serializable]
public class AuthResponseDto
{
    public bool success { get; set; }
    public string accessToken { get; set; }
    public string refreshToken { get; set; }
}
