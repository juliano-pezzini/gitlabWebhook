using Flurl;
using Flurl.Http;
using GitLabWebook.API.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitLabWebook.API.Business
{
    public class MergeRequestValidator
    {

        public IConfiguration Configuration { get; set; }

        
        public void Validate(GitLabMergeRequest mergeRequest) {

            var gitLabUrl = Configuration["GitLabUrl"];
            var gitLabPrivateToken = Configuration["GitLabPrivateToken"];
            var labelForNoLabelMR = Configuration["LabelForNoLabelMR"];

            if (string.IsNullOrEmpty(gitLabUrl)) { 
                Console.WriteLine("No GitLabUrl configured, look at appsettings.json and try again!");
                return;
            }

            if (string.IsNullOrEmpty(gitLabPrivateToken))
            {
                Console.WriteLine("No GitLabPrivateToken configured, look at appsettings.json and try again!");
                return;
            }

            if (string.IsNullOrEmpty(labelForNoLabelMR))
            {
                Console.WriteLine("No LabelForNoLabelMR configured, look at appsettings.json and try again!");
                return;
            }

            var newLabels = new List<string>();
            
            if (mergeRequest.labels.Length == 0)
            {
                Console.WriteLine("Merge request with no Label, assigning " + labelForNoLabelMR + " label");
                newLabels.Add(labelForNoLabelMR);
            }

            if (newLabels.Count > 0)
            {
                var putMRResource = new GitLabMergeRequestPutResource() { labels = newLabels.ToArray() };

                gitLabUrl
                    .AppendPathSegments("api", "v4")
                    .AppendPathSegments("projects", mergeRequest.project.id, "merge_requests")
                    .AppendPathSegment(mergeRequest.object_attributes.iid)
                    .SetQueryParam("private_token", gitLabPrivateToken)
                    .PutJsonAsync(putMRResource);
            }
        }
    }
}
