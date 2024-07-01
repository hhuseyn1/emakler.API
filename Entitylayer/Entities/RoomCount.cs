using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class RoomCount
{
    public Guid IdRoomCount { get; set; }

    public string? RoomCountName { get; set; }

    public string? Keyword { get; set; }
}
