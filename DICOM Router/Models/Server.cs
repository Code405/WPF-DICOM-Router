using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class Server
{
    public string? AeTitle { get; set; }

    public int? Port { get; set; }

    public int? LoggingMode { get; set; }
}
