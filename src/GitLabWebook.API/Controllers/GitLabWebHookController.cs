using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitLabWebook.API.Business;
using GitLabWebook.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GitLabWebook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitLabWebHookController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public GitLabWebHookController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // POST api/values
        [HttpPost]
        [Route("MergeRequest")]
        public IActionResult MergeRequest(object obj)
        {
            
            var str = obj.ToString();

            var gitLabMergeRequest =
                JsonConvert.DeserializeObject<GitLabMergeRequest>(str);

            if (gitLabMergeRequest.object_kind == "merge_request")
            {

                var validator = new MergeRequestValidator();
                validator.Configuration = Configuration;
                validator.Validate(gitLabMergeRequest);

                Console.WriteLine("Merge request validated: " + gitLabMergeRequest.object_attributes.title );
                
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
