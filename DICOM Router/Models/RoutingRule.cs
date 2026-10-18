using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class RoutingRule
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? SourceTag { get; set; }

    public string? DestTag { get; set; }

    public string? ReplaceValue { get; set; }

    public string? MatchValue { get; set; }

    public string DeviceName { get; set; } = null!;
}
