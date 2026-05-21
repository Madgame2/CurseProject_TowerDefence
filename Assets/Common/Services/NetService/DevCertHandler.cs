using UnityEngine;
using UnityEngine.Networking;

public class DevCertHandler : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; // ❗ DEV ONLY
    }
}
