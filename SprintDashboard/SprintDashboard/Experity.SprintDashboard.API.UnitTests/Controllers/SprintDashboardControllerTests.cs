using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Experity.SprintDashboard.API.UnitTests.Controllers
{
    public class TemplateControllerTests
    {
        [Test]
        public void TemplateSampleUnitTest()
        {
            var item1 = 12;
            var item2 = 12;
            Assert.That(item1, Is.AtLeast(11));
            Assert.That(item1, Is.EqualTo(item2));
        }
    }
}
