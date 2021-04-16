﻿using System.Collections.Generic;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDoItemRecord> Items { get; set; } = new();
    }
}