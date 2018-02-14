using Incidents.Models.RequestModel;
using Incidents.Models.TableStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Incidents.Controllers
{
    /// <summary>
    /// Main class to handle all the requests. Not granulated to keep it simple.
    /// </summary>
    public class MainController : ApiController
    {
        /// <summary>
        /// Simulation of my database for the "incidents" requests
        /// </summary>
        static List<Incident> registeredIncidents = new List<Incident>();


        #region LOCALITIES VALUES
        /// <summary>
        /// Simulation of the "required" values
        /// </summary>
        readonly Locality[] registeredLocalities = new Locality[]
        {
            new Locality
            {
                _id = "08f04eff03cc4510bbe646115124bd54",
                name = "Los Alcarrizos"
            },
            new Locality
            {
                _id = "5e5f9c498b4b47bda2a0f8c3eb28ced5",
                name = "Los Proceres"
            }
        };
        #endregion


        #region /GET -> incidents
        /// <summary>
        /// Get all the registered incidents
        /// </summary>
        /// <returns>Collection containing all the non archived incidents</returns>
        [HttpGet]
        [Route("incidents")]
        public IEnumerable<Incident> GetIncidents() => registeredIncidents.Where(obj => !obj.isArchived);
        #endregion


        #region /POST -> incidents
        /// <summary>
        /// Insert a new incident
        /// </summary>
        /// <param name="incident">Incident model to insert</param>
        /// <returns>Value indicating whether the required incident was inserted or no</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost]
        [Route("incidents")]
        public bool PostIncidents([FromBody] IncidentRequest incident)
        {
            #region VALIDATION
            HttpResponseMessage errorResponseMessage = null;

            #region CHECK VALUES
            if (incident == null)
                errorResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Invalid request. No model received.")
                };

            else if (incident.kind == null)
                errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent("The incident type cannot be null")
                };

            // Get a collection with the allowed values
            else if (!Enum.GetNames(typeof(Incident.AllowedIncidents)).Contains(incident.kind = incident.kind.ToUpper().Trim()))
                errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent($"Invalid incident type: \"{incident.kind}\"")
                };

            else if (incident.happenedAt == null)
                errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent("Invalid date")
                };

            else if (registeredLocalities.Count(obj => obj._id == incident.locationId) == 0)
                errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent($"Invalid location ID: \"{incident.locationId}\"")
                };
            #endregion

            if (errorResponseMessage != null)
                // Throw an error instead of returning "false" to give information to the requester
                throw new HttpResponseException(errorResponseMessage);
            #endregion

            registeredIncidents.Add(new Incident
            {
                _id = Guid.NewGuid().ToString().Replace("-", ""),
                kind = incident.kind,
                happenedAt = (DateTime)incident.happenedAt,
                isArchived = false,
                locationId = incident.locationId
            });

            return true;
        }
        #endregion


        #region /POST -> incidents/{incidentId}/archive
        /// <summary>
        /// Archive the specified incident
        /// </summary>
        /// <param name="incidentId">Incident ID</param>
        /// <returns>Value indicating whether the required incident was archived or no</returns>
        [HttpPost]
        [Route("incidents/{incidentId}/archive")]
        public bool ArchiveIncident(string incidentId)
        {
            if (registeredIncidents.Count(obj => obj._id == incidentId) == 0)
                return false;

            registeredIncidents.Where(obj => obj._id == incidentId).FirstOrDefault().isArchived = true;

            return true;
        }
        #endregion


        #region /GET -> localities
        /// <summary>
        /// Get all the registered localities
        /// </summary>
        /// <returns>Collection of all the localities</returns>
        [HttpGet]
        [Route("localities")]
        public Locality[] GetLocalities() => registeredLocalities;
        #endregion


        #region /GET -> localities/{localityId}
        /// <summary>
        /// Get an specific locality
        /// </summary>
        /// <param name="localityId">Locality ID</param>
        /// <returns>Get the requested locality</returns>
        [HttpGet]
        [Route("localities/{localityId}")]
        public Locality GetLocality(string localityId) => registeredLocalities.Where(obj => obj._id == localityId).FirstOrDefault();
        #endregion
    }
}
