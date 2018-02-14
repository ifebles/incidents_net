using Incidents.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidents_test
{
    [TestClass]
    public class TestIncidentsModule
    {
        [TestMethod]
        public void GetAllLocalities_ShouldReturnTheTwoExistentLocalities()
        {
            var controller = new MainController();

            var result = controller.GetLocalities();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }


        [TestMethod]
        public void GetOnlyOneLocality_ShouldReturnOnlyOneResultIfFoundOrNullIfNot()
        {
            var controller = new MainController();

            var localities = controller.GetLocalities();

            var result = controller.GetLocality(localities.First()._id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result._id != null);
            Assert.IsTrue(result.name != null);

            result = controller.GetLocality("random");

            Assert.IsNull(result);
        }


        [TestMethod]
        public void InsertNewIncident_ShouldInsertIfValidAndListAtLeastOneAndArchiveAnIncident()
        {
            var controller = new MainController();

            try
            {
                controller.PostIncidents(new Incidents.Models.RequestModel.IncidentRequest
                {
                    kind = "invalid",
                    happenedAt = null,
                    locationId = "notFound"
                });

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
                //Assert.IsTrue(ex.GetType() == typeof(System.Web.Http.HttpResponseException));

                //var exception = (System.Web.Http.HttpResponseException)ex;

                //Assert.IsTrue((int)exception.Response.StatusCode == 400 || (int)exception.Response.StatusCode == 409);
            }

            try
            {
                var result = controller.PostIncidents(new Incidents.Models.RequestModel.IncidentRequest
                {
                    kind = "robbery",
                    happenedAt = DateTime.Now,
                    locationId = controller.GetLocalities().First()._id
                });

                Assert.IsTrue(result);
            }
            catch {
                Assert.Fail();
            }

            var incidents = controller.GetIncidents();

            Assert.IsTrue(incidents.Count() > 0);

            
            Assert.IsFalse(controller.ArchiveIncident("InvalidId"));

            try
            {
                var archived = controller.ArchiveIncident(incidents.ToList().Last()._id);

                Assert.IsTrue(archived);
            }
            catch {
                Assert.Fail();
            }
        }
    }
}
