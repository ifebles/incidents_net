using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidents.Models.RequestModel
{
    /// <summary>
    /// Incident insertion request model
    /// </summary>
    public class IncidentRequest
    {
        public string kind { get; set; }
        public string locationId { get; set; }
        public DateTime? happenedAt { get; set; }
    }
}
