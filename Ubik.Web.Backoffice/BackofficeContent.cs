﻿using Ubik.Web.Backoffice.Contracts;

namespace Ubik.Web.Backoffice
{
    internal class BackofficeContent : IBackofficeContent
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }
    }
}