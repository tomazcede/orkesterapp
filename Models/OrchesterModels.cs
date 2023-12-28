using System;
using System.Collections.Generic;

namespace orkesterapp.Models;
public class Venue
{
    public int ID { get; set; }
    public string VenueName  { get; set; }
    public string Location  { get; set; }
}

public class Performance
{
    public int ID { get; set; }
    public int OrchesterID { get; set; }
    public int VenueID { get; set; }
    public DateTime Date { get; set; }
}

public class Orchester
{
    public int ID { get; set; }
    public string OrchestraName  { get; set; }
}