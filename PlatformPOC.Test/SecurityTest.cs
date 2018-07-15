using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlatformPOC.PlatformContracts;
using PlatformPOC.PlatformImplementation;

namespace PlatformPOC.Test
{
    [TestClass]
    public class SecurityTest
    {


        [TestMethod]
        public void ShouldSuccessfullyValidateJsonData()
        {
            //Given a valid json body
            string validJsonBody = TestResources.WellFormedJson;
            
            IPlatform platform = new Platform(null, null);

            //When we validate that it is well formed
            var result = platform.ValidateWellFormedJson(validJsonBody);
            
            //Then a success response must be returned
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldFailToValidateJsonData()
        {
            //Given a valid json body
            string validJsonBody = TestResources.NotWellFormedJson;
            
            IPlatform platform = new Platform(null, null);

            //When we validate that it is well formed
            var result = platform.ValidateWellFormedJson(validJsonBody);

            //Then a success response must be returned
            Assert.IsFalse(result);
        }
    }
}
