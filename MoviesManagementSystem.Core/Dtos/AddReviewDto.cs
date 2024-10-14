﻿using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Dtos
{
    public class AddReviewDto
    {
        public string Content { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
    }
}
