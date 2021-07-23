﻿using System;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string MediaType { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}