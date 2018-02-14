using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidents.Models.TableStructures
{
    public class Incident
    {
        public string _id { get; set; }
        public string kind { get; set; }
        public string locationId { get; set; }
        public DateTime happenedAt { get; set; }
        public bool isArchived { get; set; }

        public enum AllowedIncidents
        {
            ROBBERY,
            MURDER,
            TRAFFIC_ACCIDENT,
            SHOOTING,
            ASSAULT
        }
    }
}
