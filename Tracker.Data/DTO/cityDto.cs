﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Data.DTO
{
  public class cityDto
    {

        public int Id { get; set; }
        public string cityName { get; set; }
        public int governorateId { get; set; }
        public string governorateName { get; set; }
    }
}
