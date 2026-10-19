using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class OutgingStudy
{
    public int Id { get; set; }

    public string? DateSent { get; set; }

    public string? PatientId { get; set; }

    public string? PatientName { get; set; }

    public string? ModalityType { get; set; }

    public int? ImageCount { get; set; }

    public string? RuleName { get; set; }

    public string? DeviceName { get; set; }

    public string? Status { get; set; }

    public string? Suid { get; set; }
}
