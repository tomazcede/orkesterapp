using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace orkesterapp.Models;
public class Venue
{
    public int ID { get; set; }
    public string VenueName  { get; set; }
    public string Location  { get; set; }


    public List<Performance>? Performances { get; set; }
}

public class Performance
{
    public int ID { get; set; }
    public DateTime Date { get; set; }



    public int OrchesterID { get; set; }
    public Orchester? Orchester { get; set; }
    public int VenueID { get; set; }
    public Venue? Venue { get; set; }
}

public class Orchester
{
    public int ID { get; set; }
    public string OrchestraName  { get; set; }
    
    public List<User>? Users { get; set; }
    public List<Performance>? Performances { get; set; }
}