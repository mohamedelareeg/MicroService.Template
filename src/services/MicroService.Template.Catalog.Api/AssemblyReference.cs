﻿using System.Reflection;

namespace MicroService.Template.Catalog.Api;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
