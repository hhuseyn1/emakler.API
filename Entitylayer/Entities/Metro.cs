using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class Metro
{
    public Guid IdMetro { get; set; }

    public string? MetroName { get; set; }

    public Guid? FkIdRegion { get; set; }

    public string? Keyword01 { get; set; }

    public string? Keyword02 { get; set; }

    public string? Keyword03 { get; set; }

    public string? Keyword04 { get; set; }

    public string? Keyword05 { get; set; }
}
