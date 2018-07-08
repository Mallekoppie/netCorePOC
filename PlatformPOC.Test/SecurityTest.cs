using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlatformPOC.PlatformContracts;
using PlatformPOC.PlatformImplementation;

namespace PlatformPOC.Test
{
    [TestClass]
    public class SecurityTest
    {

        [TestMethod]
        public void ShouldSuccessFullyAuthenticateOAuth2Token()
        {
            //Given a token that is valid and has not expired
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IlNTUWRoSTFjS3ZoUUVEU0p4RTJnR1lzNDBRMCIsImtpZCI6IlNTUWRoSTFjS3ZoUUVEU0p4RTJnR1lzNDBRMCJ9.eyJhdWQiOiJodHRwczovL2dyYXBoLndpbmRvd3MubmV0IiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvOTQ3NDIwMzMtNTdhOC00OWIzLTgxYWEtZTcyODMyYTBhZWU3LyIsImlhdCI6MTUyMTM4MzYyOCwibmJmIjoxNTIxMzgzNjI4LCJleHAiOjE1MjEzODc1MjgsImFjciI6IjEiLCJhaW8iOiJBU1FBMi84R0FBQUFYVnc0Nk9oZVU4VThpUXJoZkFvRnpmNmROUHVuL1lwRDIyd0paczNWNTd3PSIsImFtciI6WyJwd2QiXSwiYXBwaWQiOiI5YTY0NTI3ZC1jNjVjLTQyZTgtYTI3Yy0xNGZhNDc4ZWNlNjUiLCJhcHBpZGFjciI6IjEiLCJmYW1pbHlfbmFtZSI6IkJhcm5hcmQiLCJnaXZlbl9uYW1lIjoiSGVpbnJpY2giLCJpcGFkZHIiOiIxOTcuMjQ1LjQ0LjEzMiIsIm5hbWUiOiJkcmlra2llIiwib2lkIjoiZWE5Y2RjMzAtOTU4NC00ZjU3LTg2OWEtMDMxOWQwYTAyNTU1IiwicHVpZCI6IjEwMDMyMDAwMEJFREJBNjgiLCJzY3AiOiJEaXJlY3RvcnkuQWNjZXNzQXNVc2VyLkFsbCBEaXJlY3RvcnkuUmVhZC5BbGwgRGlyZWN0b3J5LlJlYWRXcml0ZS5BbGwgR3JvdXAuUmVhZC5BbGwgR3JvdXAuUmVhZFdyaXRlLkFsbCBNZW1iZXIuUmVhZC5IaWRkZW4gVXNlci5SZWFkIFVzZXIuUmVhZC5BbGwgVXNlci5SZWFkQmFzaWMuQWxsIiwic3ViIjoiWTNFejk0RU9COUctX2VVcHB5dzBfaTVCS3c1eDhSTHJIRVdVaWV3RFZnRSIsInRlbmFudF9yZWdpb25fc2NvcGUiOiJBRiIsInRpZCI6Ijk0NzQyMDMzLTU3YTgtNDliMy04MWFhLWU3MjgzMmEwYWVlNyIsInVuaXF1ZV9uYW1lIjoiZHJpa2tpZUBtYWxsZWtvcHBpZS5vbm1pY3Jvc29mdC5jb20iLCJ1cG4iOiJkcmlra2llQG1hbGxla29wcGllLm9ubWljcm9zb2Z0LmNvbSIsInV0aSI6InpqUWFXMUw1UlVHMGl4dnBLLVEyQUEiLCJ2ZXIiOiIxLjAifQ.Tkc6yz4EvvP1QT-lyCnrPNykZY_GdlZPtBiR4g9fD6Y2ShG9ySZE8TxZfNK3OmUWQHjMFW7XNbH7SDYiaaCH-MsqT0d4CbVm3EI99TUX_WUgA1TPtJYgfoy7MtHESTRH_rH2oaa9UjH47jNhRrgs6RmyM6_yrJ-AgCfBVm2RiHe6Frv2FnS0maqbo0Rs4hIZoTtFDza0MTTTgXcnhhzbXgIz4AokR_2KOAvnupqSGhwgIyAtI6ZKURwCv7j8G2mOt9_o1hToAT1Ad4zGTQZaOatPcSeXUW8pvSYPx1g5M7HTVHNzUbY1weJ5OWc4VfgdZgjjBpnHOv7TTkLTe-Du6Q";            
            IPlatform platform = new Platform(null);

            //When we call the platform to validate it
            var validationResult = platform.ValidateOAuth2Token(token);

            //Then a success response will be returned
            Assert.IsTrue(validationResult);
        }

        [TestMethod]
        public void ShouldSuccessfullyValidateJsonData()
        {
            //Given a valid json body
            string validJsonBody = TestResources.WellFormedJson;
            
            IPlatform platform = new Platform(null);

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
            
            IPlatform platform = new Platform(null);

            //When we validate that it is well formed
            var result = platform.ValidateWellFormedJson(validJsonBody);

            //Then a success response must be returned
            Assert.IsFalse(result);
        }
    }
}
