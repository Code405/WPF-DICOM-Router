using System;
using System.Collections.Generic;

namespace DICOM_Router.Models;

public partial class IncomingStudy
{
    public int Id { get; set; }

    public string? DateRec { get; set; }

    public string? PatientId { get; set; }

    public string? PatientName { get; set; }

    public string? ModalityType { get; set; }

    public int? ImageCount { get; set; }

    public string? Suid { get; set; }
}
