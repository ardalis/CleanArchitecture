using System.Collections.Generic;
using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class ProjectListResponse
    {
        public List<ProjectRecord> Projects { get; set; } = new();
    }
}
