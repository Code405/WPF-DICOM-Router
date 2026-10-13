using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class Device
{
    public int? Id { get; set; }

    public string? DeviceName { get; set; }

    public string? IpAddress { get; set; }

    public string? CallingAe { get; set; }

    public string? CalledAe { get; set; }

    public int? Port { get; set; }
}
