using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class Device
{
    public int Id { get; set; }

    public string DeviceName { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public string CallingAe { get; set; } = null!;

    public string CalledAe { get; set; } = null!;

    public int Port { get; set; }
}
