﻿using System;
namespace Elearn.Helpers
{
    public class EmailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}