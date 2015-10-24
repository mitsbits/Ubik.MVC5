﻿using System;

namespace Ubik.Web.Infra.Contracts
{
    public interface ISlugifier
    {
        string SlugifyText(string text);

        string SlugifyDate(DateTime date);

        DateTime DateFromSlug(string slug);
    }
}