using Microsoft.AspNetCore.Mvc;

namespace GrpcApi.Controllers
{
    public class SecretController : Controller
    {
        [Route("/secret")]
        public string Secret()
        {
            return "secret";
        }
    }
}